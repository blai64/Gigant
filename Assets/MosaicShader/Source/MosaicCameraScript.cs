using UnityEngine;


public class MosaicCameraScript : MonoBehaviour {
	
	public static MosaicCameraScript instance;
	public bool depixelizing;
	public bool pixelizing;

	public bool isMosaic;
	public GameObject white;
	SpriteRenderer renderer;
	private Color rendererAlpha;

	private Vector3 targetPosition;
	private BoxCollider2D targetBounds;

    // The material containing the mosaic shader
    private Material material;

	// idle when enters a new scene.
	public GameObject player;
	private Animator anim;

    public Shader mosaicShader;

    /**
    ** Define the inputs for the uniforms of the mosaic shader
    **/

    // The angle in degrees used to rotate the mosaics
    [Range(0.0f, 360.0f)]
    public float Angle;
    // The relative amount that each mosaic should shrink
    [Range(0, 1)]
    public double Shrink = 0.3;

    // The X and Y size of the mosaics
    public Vector2 TileSize = new Vector2(20, 20);

    // The opacity of the mosaics
    [Range(0, 1)]
	public float MosaicOpacity = 0.0f;
    // The opacity of the actual rendered scene
    [Range(0, 1)]
    public float SceneOpacity = 0.0f;

    public int AvgTextureTileSizeX = 1;
    public int AvgTextureTileSizeY = 1;


    void Start() {
		anim = player.GetComponentInChildren<Animator> ();
		isMosaic = true;
		depixelizing = false;
		pixelizing = false;
		//0427 
		//fade 
		rendererAlpha = white.GetComponent<SpriteRenderer> ().color;
		rendererAlpha.a = 1.0f;
		MosaicOpacity = 2.0f;
		targetPosition = PlayerController.instance.transform.position;
		targetBounds = MainCamera.instance.cameraBounds;
		white.GetComponent<SpriteRenderer> ().color = rendererAlpha;
		material.SetFloat("_MosaicOpacity", MosaicOpacity);
	}

    void Awake() {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (this);
		}
		material = new Material(mosaicShader);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
		
		if (isMosaic) {
			// Generate a texture containings the average colors of the rendered scene per tile
			int w = source.width / AvgTextureTileSizeX;
			int h = source.height / AvgTextureTileSizeY;

			var lowRez = RenderTexture.GetTemporary (w, h);
			Graphics.Blit (source, lowRez);
			// Set the generated texture as uniform for the shader
			material.SetTexture ("_AverageTex", lowRez);
			// Mosaic the scene
			Graphics.Blit (source, destination, material);

			// Release temporary texture
			RenderTexture.ReleaseTemporary (lowRez);
		}
    }

    void Update() {
		
		if (pixelizing && !depixelizing) {
			isMosaic = true;
			if (PlayerController.instance.longerTransition) {
				MosaicOpacity += 0.05f;
				rendererAlpha.a += 0.025f;
			} else {
				MosaicOpacity += 0.1f;
				rendererAlpha.a += 0.05f;
			}

			white.GetComponent<SpriteRenderer> ().color = rendererAlpha;
//			anim.ResetTrigger ("isHurt");
//			anim.ResetTrigger ("isDead");
//			anim.ResetTrigger ("isAttacking");
		}

		// Move camera
		if (rendererAlpha.a >= 1.0f) {
			depixelizing = true;
			pixelizing = false;
			PlayerController.instance.transform.position = targetPosition;
			PlayerController.instance.GetComponent<Rigidbody2D> ().velocity = new Vector2(0,0);
			MainCamera.instance.UpdateBounds(targetBounds);
			PlayerController.instance.Disable (true);
			BaseEnemyBehavior[] enemies = FindObjectsOfType(typeof(BaseEnemyBehavior)) as BaseEnemyBehavior[];
			BaseAdvancedEnemyBehavior[] enemies2 = FindObjectsOfType(typeof(BaseAdvancedEnemyBehavior)) as BaseAdvancedEnemyBehavior[];
			foreach (BaseEnemyBehavior enemy in enemies) {
				enemy.Reset ();
			}
			foreach (BaseAdvancedEnemyBehavior enemy in enemies2) {
				enemy.Reset ();
			}

		}

		if (depixelizing && !pixelizing) {
			if (PlayerController.instance.longerTransition) {
				MosaicOpacity -= 0.05f;
				rendererAlpha.a -= 0.025f;
			} else {
				MosaicOpacity -= 0.1f;
				rendererAlpha.a -= 0.05f;
			}
			white.GetComponent<SpriteRenderer> ().color = rendererAlpha;
			anim.SetBool ("isRunning", false);

		}

		if (MosaicOpacity <= 0.0f && depixelizing) {
			//PlayerController.instance.longerTransition = false;
			isMosaic = false;
			depixelizing = false;
			PlayerController.instance.Enable(true);

		} else if(MosaicOpacity > 0.0f) {

		}

		// Set the uniforms
        material.SetFloat("_Angle", (float)(Angle / 180.0) * Mathf.PI);
        material.SetFloat("_Shrink", (float)Shrink);
        material.SetVector("_TileSize",TileSize);
        material.SetFloat("_MosaicOpacity", MosaicOpacity);
        material.SetFloat("_SceneOpacity", SceneOpacity);
    }

	public void SetTargetPosition(Vector3 newTarget, BoxCollider2D newBounds) {
		PlayerController.instance.Disable(true);
		targetPosition = newTarget;
		targetBounds = newBounds;
		pixelizing = true;
	}
}

