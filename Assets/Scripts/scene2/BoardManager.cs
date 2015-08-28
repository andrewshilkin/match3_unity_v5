using UnityEngine;
using System.Collections;
using System.Timers;



public class BoardManager : MonoBehaviour {


    public int countX = 5;
    public int countY = 5;

    private float gemSizeWidth =  1.2f;
    private float gemSizeHeight = 1.2f;

    public int spacing = 15;

    private GameObject[,] gemsBoard;

    public Object[] gems;

    private GameObject swapGem;

    public GameObject text;

    public GameObject gemSelect;


    private ArrayList _matches = new ArrayList();



    void Awake()
    {
        gems = Resources.LoadAll("Gems");

        PopupScore.getInstance().initTarget(gameObject);
        
        gemsBoard = new GameObject[countX, countY];

        CreateField();

        CreateBoard();

    }
    public void onScoreScale(object arg)
    {
        if(arg is GameObject)
        {
            Destroy((GameObject)arg);
        }
    }


	// Use this for initialization
	void Start () {

	}

    void OnDestroy()
    {
        
    }
	
	// Update is called once per frame
	void Update () 
    {
        //GemController.getInstance().OnUpdate();

        if (lookMatches().Count != 0)
        {
            //findAndRemoveMatches();
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
            
	}

    public void CreateField()
    {

        float posX = gameObject.transform.position.x;
        float posY = gameObject.transform.position.y;

        for (int i = 0; i < countX; i++)
        {
            posX = gameObject.transform.position.x;
            posY = gameObject.transform.position.y - 1.2f * i;

            for (int j = 0; j < countY; j++)
            {
                posX = gameObject.transform.position.x + 1.2f * j;

                GameObject field = CreateFieldPlate((i+j) % 2 > 0);
                field.transform.parent = transform;
                field.transform.localScale = new Vector3(1, 1);
                field.transform.position = new Vector3(posX, posY, 1);
            }
        }
    }

    public GameObject CreateFieldPlate(bool isLight = false)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Candy/Field");

        GameObject gameObject = new GameObject("field");
        gameObject.transform.localScale = new Vector3(1, 1, 1);

        SpriteRenderer renderer = gameObject.AddComponent<SpriteRenderer>();
        int index = (isLight?0:1);
        renderer.sprite = sprites[index];

        return gameObject;
    }

    public void CreateBoard()
    {
        Debug.Log("CreateBoard()");
        GameObject[,] gems;
        try
        {

        } 
        catch(System.Exception error)
        {
            Debug.Log("FUUU "+ error.Message);
        }

        while (true)
        {
            gems = createBoadGems();

            if (lookMatches().Count != 0)
            {
                continue;
            }
            if (lookForPossibles() == false)
            {
                continue;
            }

            break;

        }

        placeBoardGems(gems);
           
    }
    private void placeBoardGems(GameObject[,] gems)
    {

        gemsBoard = new GameObject[countX, countY];
        //
        string[][] strs = new string[countX][];

        
        //
        float posX = gameObject.transform.position.x;
        float posY = gameObject.transform.position.y;

        for (int i = 0; i < countX; i++)
        {
            posX = gameObject.transform.position.x;
            posY = gameObject.transform.position.y - 1.2f * i ;

            string[] strs2 = new string[countX];

            for (int j = 0; j < countY; j++)
            {
                GameObject gem = (GameObject)Instantiate(gems[i, j], new Vector3(0, 0, 0), Quaternion.identity);
                gem.transform.parent = gameObject.transform;
                gem.transform.localScale = new Vector3(1, 1);
                

                posX = gameObject.transform.position.x + 1.2f * j ;

                gem.transform.position = new Vector3(posX, posY, 0);

                strs2[j] = gem.name;

                Gem gemClass = (gem.GetComponent<Gem>());

                gemClass.onMouseClick += onGemClicked;
                gemClass.onDestroyed += onGemDestroyed;
                //gemClass.onDropDown += onDropDown;

                gemClass.row = j;
                gemClass.col = i;

                gemsBoard[i, j] = gem;
                

            }

            strs[i] = strs2;

        }
        for (int i = 0; i < strs.Length; i++)
        {
            //Debug.Log(string.Join(",",strs[i]));

        }
        Debug.Log("CreateBoard() " + lookMatches().Count.ToString());
    }

    private GameObject[,] createBoadGems()
    {
       
        System.Array.Clear(gemsBoard, 0, gemsBoard.Length);
        GameObject[,] gems = new GameObject[countX, countY];
        for (int i = 0; i < countX; i++)
        {
            for (int j = 0; j < countY; j++)
            {
                GameObject gem = (GameObject)randomGem();

                gemsBoard[i, j] = gem;
                gems[i, j] = gem;
            }
        }
/*
        int[,] stite = new int[,] 
        {
            
            { "Candy5" ,"Candy10" ,"Candy6" ,"Candy7" ,"Candy10"}, 
            {"Candy9" ,"Candy5" ,"Candy3" ,"Candy3" ,"Candy2"}, 
            {"Candy10" ,"Candy9" ,"Candy9" ,"Candy2" ,"Candy6"}, 
            {"Candy6" ,"Candy2" ,"Candy9" ,"Candy1" ,"Candy1"},
 
            {"Candy10" ,"Candy8" ,"Candy4" ,"Candy9" ,"Candy2"}
             
            {5,10,6,7,10}, 
            {9,5,3,3,2},
            {10,9,9,2,6},
            {6, 2,9,1,1},
            {10,8,4,9,2},
        };
        GameObject[,] gems = new GameObject[countX, countY];
        for (int i = 0; i < stite.GetLength(0); i++)
        {
            for (int j = 0; j < stite.GetLength(1); j++)
            {
                gemsBoard[i, j] = gems[i, j] = (GameObject)this.gems[stite[i, j] - 1];
            }
        }
*/
        return gems;
    }

    private ArrayList lookMatches()
    {
        ArrayList matchesList = new ArrayList();

        // поиск горизонтальных линий    
        for (int i = 0; i < countX; i++)
        {
            for (int j = 0; j < countY; j++)
            {
                ArrayList match = getHorLines(i, j);
                if (match.Count > 2)
                {
                    matchesList.Add(match);
                    i += match.Count - 1;
                }
            }
        }

        //
        for (int j = 0; j < countY; j++)
        {
            for (int i = 0; i < countX; i++)
            {
                ArrayList match = getVertLines(i, j);
                if (match.Count > 2)
                {
                    matchesList.Add(match);
                    j += match.Count - 1;
                }
            }
        }

        return matchesList;
    }

    private ArrayList getHorLines(int col, int row)
    {
        ArrayList match = new ArrayList();
        if (gemsBoard[col, row])
        {
            //return new ArrayList();//some error, board
        }
        match.Add(gemsBoard[col, row]);

        for (int i = 1; col + i < countX; i++)
        {
            if(gemsBoard[col, row] == null || gemsBoard[col + i, row] == null)
            {
                continue;
            }
            Gem gem1 = gemsBoard[col, row].GetComponent<Gem>();
            Gem gem2 = gemsBoard[col + i, row].GetComponent<Gem>();
            if (gem1.type == gem2.type)
            {
                match.Add(gemsBoard[col + i, row]);
            }
            else
            {
                return match;
            }
        }
        return match;
    }

    private ArrayList getVertLines(int col, int row)
    {
        ArrayList match = new ArrayList();
        match.Add(gemsBoard[col, row]);

        for (int i = 1; row + i < countX; i++)
        {
            if(gemsBoard[col, row] == null || gemsBoard[col, row + i] == null)
            {
                return new ArrayList();
            }
            Gem gem1 = gemsBoard[col, row].GetComponent<Gem>();
            Gem gem2 = gemsBoard[col, row + i].GetComponent<Gem>();
            if (gem1.type == gem2.type)
            {
                match.Add(gemsBoard[col, row + i]);
            }
            else
            {
                return match;
            }
        }
        return match;
    }

    private Object randomGem()
    {
        return gems[Random.Range(0, gems.Length)];
    }

    private GameObject _toSwap = null;
    private void onGemClicked(GameObject gem)
    {    
        Debug.Log("BoardManager.Gem clicked");
        Gem swapClass = null;

        if (_toSwap == null)
        {
            Debug.Log("BoardManager.to swap");
            _toSwap = gem;

            Vector3 pos = new Vector3(_toSwap.transform.position.x, _toSwap.transform.position.y, -1);
            // TODO gemSelect.transform.position = pos;
        }
        else
        {
            swapClass = (_toSwap.GetComponent<Gem>());

            Gem gemClass = (gem.GetComponent<Gem>());
            if (
                ((swapClass.col == gemClass.col) && (swapClass.row == gemClass.row + 1 || swapClass.row == gemClass.row - 1)) ||
                ((swapClass.row == gemClass.row) && (swapClass.col == gemClass.col + 1 || swapClass.col == gemClass.col - 1))
                 )
            {
                Debug.Log("BoardManager.swap");
                swapGems(gem, _toSwap);

                if(lookMatches().Count <= 0)
                {
                    swapGems(_toSwap, gem);

                    _toSwap = gem;
                    Vector3 pos = new Vector3(_toSwap.transform.position.x, _toSwap.transform.position.y, -1);
                    // TODO gemSelect.transform.position = pos;
                }
                else
                {
                    testPosSwap(gem, _toSwap);
                    // TODO
                    //Vector3 pos = new Vector3(gemSelect.transform.position.x, gemSelect.transform.position.y, -10);
                    //gemSelect.transform.position = pos;

                    findAndRemoveMatches();

                    _toSwap = null;
                }


                //dropGems();
            }
            else
            {
                Debug.Log("BoardManager.to swap");
                _toSwap = gem;

                Vector3 pos = new Vector3(_toSwap.transform.position.x, _toSwap.transform.position.y, -1);
                // TODO gemSelect.transform.position = pos;

            }

        }
    }

    private void swapGems(GameObject gem1, GameObject gem2)
    {
        swapPieces(gem1, gem2);

        // проверяем, был ли обмен удачным    
        if (lookMatches().Count == 0)
        {
            swapPieces(gem1, gem2);
        }
        else
        {
            //isSwapping = true;    
        }

    }

    public void swapPieces(GameObject gem1, GameObject gem2)
    {
        Gem gemClass1 = (gem1.GetComponent<Gem>());
        Gem gemClass2 = (gem2.GetComponent<Gem>());

        // обмениваем значения row и col    
        int tempCol = gemClass1.col;
        int tempRow = gemClass1.row;

        Debug.Log("1 gem1 " + gemClass1.col + " " + gemClass1.row);
        Debug.Log("1 gem2 " + gemClass2.col + " " + gemClass2.row);

        gemClass1.col = gemClass2.col;
        gemClass1.row = gemClass2.row;

        gemClass2.col = tempCol;
        gemClass2.row = tempRow;

        // изменяем позицию в сетке (grid)    
        gemsBoard[gemClass1.col, gemClass1.row] = gem1;
        gemsBoard[gemClass2.col, gemClass2.row] = gem2;

        Debug.Log("2 gem1 " + gemClass1.col + " " + gemClass1.row);
        Debug.Log("2 gem2 " + gemClass2.col + " " + gemClass2.row);
    }

    private void testPosSwap(GameObject gem1, GameObject gem2)
    {
        float tmpX = gem1.transform.position.x;
        float tmpY = gem1.transform.position.y;

        gem1.transform.position = gem2.transform.position;

        gem2.transform.position = new Vector3(tmpX, tmpY);
    }

    public void findAndRemoveMatches()
    {
        // формируем список линий    
        ArrayList matches = lookMatches();
        for (int i = 0; i < matches.Count; i++)
        {
            ArrayList item = (ArrayList)matches[i];
            int numPoints = (item.Count - 1) * 25;

            for (int j = 0; j < item.Count; j++)
            {
                if (item[j] != null)
                {
                    
                    GameObject gem = (GameObject)item[j];
                    Gem gemClass = gem.GetComponent<Gem>();

                    PopupScore.getInstance().AddPopupScore(new Vector3(gem.transform.position.x, gem.transform.position.y, -2), numPoints);

                    Debug.Log("Destroy gem " + gemClass.col + " " + gemClass.row);
                    gemClass.destroyed = true;

                    gemsBoard[gemClass.col, gemClass.row] = null;

                    _matches.Add(new int[2]{gemClass.col, gemClass.row});

                    
                }
                
            }
            
        }
        

        //addNewPieces();
    }

    

    public void affectAbove(int col, int row)
    {
        for (int i = col - 1; i >= 0; i--)
        {
            if (gemsBoard[i, row] != null)
            {
                gemsBoard[i, row].GetComponent<Gem>().col++;
                gemsBoard[i + 1, row] = gemsBoard[i, row];

                Debug.Log("affect above replace " + (i + 1) + " " + row);

                gemsBoard[i, row] = null;

                float posX = gemsBoard[i + 1, row].transform.position.x;
                float posY = gameObject.transform.position.y - 1.2f * (i+1);
                Debug.Log("affect above coord " + posX + " " + posY);
                iTween.MoveTo(gemsBoard[i + 1, row], iTween.Hash("y", posY, "oncomplete", "onDropDown", "oncompletetarget", this.gameObject, "oncompleteparams", gemsBoard[i + 1, row]));
            }
        }
    }

    private void onGemDestroyed(GameObject gem)
    {
        if(_matches.Count > 0)
        {
            foreach(int[] pos in _matches)
            {
                affectAbove(pos[0], pos[1]);
            }

            _matches = new ArrayList();

            addNewPieces();
        }


    }

    public void addNewPieces()
    {
        for (int j = countY-1; j >= 0; j--)
        {
            int missingPieces = 0;
            for (int i = countX-1; i >= 0; i--)
            {
                if (gemsBoard[i, j] == null)
                {
                    missingPieces++;

                    float posX = gameObject.transform.position.x + gemSizeWidth * j ;
                    float posY = gameObject.transform.position.y + 5 + (missingPieces * gemSizeHeight );

                    GameObject newPiece = (GameObject)Instantiate(randomGem(), new Vector3(posX, posY, 0), Quaternion.identity);
                    newPiece.GetComponent<Gem>().col = i;
                    newPiece.GetComponent<Gem>().row = j;
                    newPiece.transform.parent = gameObject.transform;
                    newPiece.transform.position = new Vector3(posX, posY, -1);

                    newPiece.GetComponent<Gem>().isDroping = true;

                    posX = gameObject.transform.position.x + gemSizeWidth * j ;
                    posY = gameObject.transform.position.y - gemSizeHeight * i ;

                    iTween.MoveTo(newPiece, iTween.Hash("y", posY, "oncomplete", "onDropDown", "oncompletetarget", this.gameObject, "oncompleteparams", newPiece));
                    gemsBoard[i, j] = newPiece;

                    newPiece.GetComponent<Gem>().onMouseClick += onGemClicked;
                    newPiece.GetComponent<Gem>().onDestroyed += onGemDestroyed;

                    Debug.Log("adde new " + i + " " + j);
                }
            }
            missingPieces = 0;
        }


    }

    public void onDropDown(object arg)
    {
        Debug.Log("onDropDown ");
        if(!(arg is GameObject))
        {
            return;
        }

        GameObject gameObject = (GameObject)arg;
        gameObject.GetComponent<Gem>().isDroping = false;

        bool isAllDroped = true;
        for (int i = 0; i < countX; i++)
        {
            for (int j = 0; j < countY; j++)
            {
                if(gemsBoard[i,j] != null)
                {
                    if(gemsBoard[i,j].GetComponent<Gem>().isDroping)
                    {
                        isAllDroped = false;
                    }
                }
                else
                {
                    isAllDroped = false;
                }
            }
        }

        if(isAllDroped && lookMatches().Count > 0)
        {
            findAndRemoveMatches();
        }
        else if(isAllDroped && lookMatches().Count == 0)
        {
            if (lookForPossibles() == false)
            {
                MoveAllToCenter();
            }
        }
            

    }
    private void MoveAllToCenter()
    {
        for (int i = 0; i < countX; i++)
        {
            for (int j = 0; j < countY; j++)
            {

                float posX = gameObject.transform.position.x + 1.2f * 2;
                float posY = gameObject.transform.position.y - 1.2f * 2;

                Hashtable hash = iTween.Hash("x", posX,"y", posY, "oncompletetarget", this.gameObject);
                if(i == 0 && j == 0)
                {
                    hash.Add("oncomplete", "Shuffles");
                }
                iTween.MoveTo(gemsBoard[i, j], hash);
            }
        }
        
    }

    // проверка на возможные ходы по составлению линий на поле   
    public bool lookForPossibles() 
    {    
       for(int col = 0; col < countX; col++) 
       {
          for (int row = 0; row < countY; row++) 
          { 
            
             // воможна горизонтальная, две подряд      
             if (matchPattern(col, row, 
                 new int[] {1,0} , 
                 new int[,]{ {-2,0}, {-1,-1}, {-1,1}, {2,-1}, {2,1}, {3,0} } 
                 ) 
             ) 
             {       
                return true;      
             }            
            
             // воможна горизонтальная, две по разным сторонам      
             if (matchPattern(col, row, 
                 new int[] {2,0} , 
                 new int[,]{ {1,-1}, {1,1} } 
                 )
             ) 
             {       
                return true;      
             }            
 
             // возможна вертикальная, две подряд      
             if (matchPattern(col, row,
                 new int[] {0,1} ,
                 new int[,]{ {0,-2}, {-1,-1}, {1,-1}, {-1,2}, {1,2}, {0,3} } 
                )
             )
             {
                return true;      
             }            

             // воможна вертикальная, две по разным сторонам       
             if (matchPattern(col, row, 
                 new int[] {0,2} ,
                 new int[,]{ {-1,1}, {1,1} }  
                 )
                 )
             {
                return true;      
             } /*
             */
          }    
       }        
 
       // не найдено возможных линий    
       return false;   
    }  
    public bool matchPattern(int col, int row, int[] mustHave, int[,] needOne) 
    {    

        string thisType = "";
        thisType = gemsBoard[col, row].GetComponent<Gem>()._type;    
 
        // убедимся, что есть вторая фишка одного типа    
        int i = 0; 
        if (!matchType(col + mustHave[0], row + mustHave[1], thisType)) {      
            return false;     
        }    
              
 
        // убедимся,  что третья фишка совпадает по типу с двумя другими    
        for(i = 0;i<needOne.GetLength(0);i++) {     
            if (matchType(col+needOne[i,0], row+needOne[i,1], thisType)) {      
                return true;     
            }    
        }    
        return false;   
    }

    public bool matchType(int col,int row,string type) 
    {    
        // убедимся, что фишка не выходит за пределы поля    
        if ((col < 0) || (col > countX - 1) || (row < 0) || (row > countY - 1)) 
            return false;    
        return (gemsBoard[col,row].GetComponent<Gem>()._type == type);   
    }  


    private void Shuffles()
    {
        GameObject[][] arr = new GameObject[countX][];
        for (int i = 0; i < countX; i++)
        {
            arr[i] = new GameObject[countY];

            for (int j = 0; j < countY; j++)
            {
                if (gemsBoard[i, j] != null)
                {
                    arr[i][j] = gemsBoard[i, j];
                }
            }

            Shuffle(arr[i]);
        }

        for (int i = 0; i < countX; i++)
        {
            for (int j = 0; j < countY; j++)
            {
                gemsBoard[i, j] = arr[i][j];
                gemsBoard[i, j].GetComponent<Gem>().col = i;
                gemsBoard[i, j].GetComponent<Gem>().row = j;
            }
        }

        if (lookForPossibles() == false)
        {
            Debug.Log("Shuffled bad");
            Shuffles();
        }            
        else
        {
            float posX, posY = 0;

            for (int i = 0; i < countX; i++)
            {
                posX = gameObject.transform.position.x;
                posY = gameObject.transform.position.y - 1.2f * i;

                for (int j = 0; j < countY; j++)
                {
                    posX = gameObject.transform.position.x + 1.2f * j;
                    iTween.MoveTo(gemsBoard[i, j], iTween.Hash("x", posX, "y", posY, /*"oncomplete", "",*/ "oncompletetarget", this.gameObject));
                }
            }
            Debug.Log("Shuffled good");
        }
            
    }
    private void Shuffle<T>(T[] array)
    {
        int n = array.Length;
        for (int i = 0; i < n; i++)
        {
            int r = i + (int)(Random.value * (n - i));
            T t = array[r];
            array[r] = array[i];
            array[i] = t;
        }
    }
}
