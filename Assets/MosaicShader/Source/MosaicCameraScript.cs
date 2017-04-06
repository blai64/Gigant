using UnityEngine;

namespace BlackImpSoftware.MosaicShader.Source
{
    public class MosaicCameraScript : MonoBehaviour
    {
		/* Move Camera */
		public Camera myCam;
		public Transform camPos1;
		//public float speed;
		//private float startTime;
		//private float journeyLength;
		private bool depixelizing;
		private bool pixelizing;

		public bool isMosaic;
		public GameObject white;
		SpriteRenderer renderer;
		private Color tmp;
		public GameObject health;

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
			myCam = Camera.main;
			//startTime = Time.time;
			//journeyLength = Vector3.Distance (myCam.transform.position, camPos1.transform.position);
			tmp = white.GetComponent<SpriteRenderer> ().color;
			tmp.a = 0f;
			white.GetComponent<SpriteRenderer> ().color = tmp;
			//isMoving = false;
			//arrived = false;
			depixelizing = false;
			pixelizing = false;
		}

        void Awake()
        {
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
			pixelizing = triggerscenechange.instance.isColliding;

			timer = State.instance.timer;

			if (pixelizing && !depixelizing) {
				isMosaic = true;
				MosaicOpacity += 0.1f;
				tmp.a += 0.05f;
				white.GetComponent<SpriteRenderer> ().color = tmp;
				//health.SetActive (false);

				//isMoving = true;
			}


			// Move camera
			if (tmp.a >= 0.9f) {
				/*float distCovered = (Time.time - startTime) * speed;
				float fracJourney = distCovered / journeyLength;
				myCam.transform.position = Vector3.Lerp (myCam.transform.position, camPos1.transform.position, fracJourney);
				*/
				depixelizing = true;
				pixelizing = false;
				myCam.transform.position = camPos1.transform.position;
			}

			if (depixelizing && MosaicOpacity > 0f) {
				MosaicOpacity -= 0.1f;
				tmp.a -= 0.05f;
				white.GetComponent<SpriteRenderer> ().color = tmp;
			}

			if (MosaicOpacity <= 0.0f) {
				health.SetActive (true);
				isMosaic = false;
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
    }
}
