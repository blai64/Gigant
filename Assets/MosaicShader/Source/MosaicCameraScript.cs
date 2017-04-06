using UnityEngine;


public class MosaicCameraScript : MonoBehaviour
{
	public static MosaicCameraScript instance;
	private bool depixelizing;
	private bool pixelizing;

	public bool isMosaic;
	public GameObject white;
	SpriteRenderer renderer;
	private Color rendererAlpha;
	public GameObject health;


	private Vector3 targetPosition;

    // The material containing the mosaic shader
    private Material material;
	private float timer;

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


    void Start()
    {
		isMosaic = false;
		depixelizing = false;
		pixelizing = false;
		//fade 
		rendererAlpha = white.GetComponent<SpriteRenderer> ().color;
		rendererAlpha.a = 0f;
		white.GetComponent<SpriteRenderer> ().color = rendererAlpha;
	}

    void Awake()
    {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (this);
		}
		material = new Material(mosaicShader);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
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

    void Update()
    {

		timer = State.instance.timer;

		if (pixelizing && !depixelizing) {
			isMosaic = true;
			MosaicOpacity += 0.1f;
			rendererAlpha.a += 0.05f;
			white.GetComponent<SpriteRenderer> ().color = rendererAlpha;
		}


		// Move camera
		if (rendererAlpha.a >= 0.9f) {
			depixelizing = true;
			pixelizing = false;
			PlayerController.instance.transform.position = targetPosition;
		}

		if (depixelizing && MosaicOpacity > 0f) {
			MosaicOpacity -= 0.1f;
			rendererAlpha.a -= 0.05f;
			white.GetComponent<SpriteRenderer> ().color = rendererAlpha;
		}

		if (MosaicOpacity <= 0.0f) {
			health.SetActive (true);
			isMosaic = false;
			depixelizing = false;
		} else {
			health.SetActive (false);
		}
			




		// Set the uniforms
        material.SetFloat("_Angle", (float)(Angle / 180.0) * Mathf.PI);
        material.SetFloat("_Shrink", (float)Shrink);
        material.SetVector("_TileSize",TileSize);
        material.SetFloat("_MosaicOpacity", MosaicOpacity);
        material.SetFloat("_SceneOpacity", SceneOpacity);
    }

	public void SetTargetPosition(Vector3 newTarget){
		targetPosition = newTarget;
		pixelizing = true;
	}
}

