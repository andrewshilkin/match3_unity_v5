using UnityEngine;
using System.Collections;

public class Gem : MonoBehaviour {

    public int col = 0;
    public int row = 0;


    public string type = "";

    virtual public string _type
    {
        get { return type; }
    }

    private bool _destroyed = false;
    public bool destroyed
    {
        set
        {
            if (_destroyed != value)
            {
                _destroyed = value;

                animator.StopPlayback() ;

                animator.SetBool("GemDestroyed", _destroyed);

                
            }
        }
    }

    public bool isDroping = false;

    /*protected Vector2 _position = new Vector2(0, 0);*/
    public Vector3 position
    {
        get
        {
            return gameObject.transform.position;
        }
        set
        {
            gameObject.transform.position = value;
        }

    }

    public SpriteRenderer spriteRenderer;

    public BoxCollider2D boxCollider;
    public Animator animator;

    public delegate void GemClickMethod(GameObject gem);
    public event GemClickMethod onMouseClick;

    public delegate void GemDestroyedMethod(GameObject gem);
    public event GemDestroyedMethod onDestroyed;



    void Awake()
    {
        animator = GetComponent<Animator>();

        //InputAggregator.OnTeleportEvent += test;
    }

	// Use this for initialization
	void Start () 
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        
        
	}

    
	// Update is called once per frame
	void Update () 
    {
        OnUpdate();
            
	}

    public void OnUpdate()
    {

    }

    protected void DrawQuad(Rect rect, Color color)
    {
        Texture2D texture = new Texture2D( 1, 1 );
        texture.SetPixel(0, 0, color);
        texture.Apply();
        GUI.skin.box.normal.background = texture;
        GUI.Box(rect, GUIContent.none);
    }

    void OnGUI()
    {

    }

    void OnMouseDown()
    {
        if (gameObject != null && onMouseClick != null)
        {
            onMouseClick(gameObject);
        }
      
        Debug.Log("Mouse down " + ToString());
    }

    public override string ToString()
    {
        return "gem " + col + " " + row;
    }

    public void onDestroyAnimationComplete()
    {
        Debug.Log("onDestroyAnimationComplete " + col + " " + row);

        if (gameObject != null && onDestroyed != null)
        {
            onDestroyed(gameObject);
            Destroy(gameObject);
        }
            
    }

    public void onDropDownComplete()
    {
        Debug.Log("onDropDown " + col + " " + row);
/*
        if (gameObject != null && onDropDown != null)
        {
            onDropDown(gameObject);
        }
 */
    }
    
}
