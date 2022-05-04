using System;
using MySql.Data.MySqlClient;
using UnityEngine;
using UnityEngine.UI;

public class Test1 : MonoBehaviour
{
    public Button DbButton;
    public GameObject player;
    public string server = "127.0.0.1";
    public string userid = "root";
    public string password = "j660982";
    public string database = "game_schema";
    public string port = "3308";
    public Button Save1;
    public Button Save2;
    public Button Save3;
    public Button Save4;
    public Button Save5;
    private Vector3 s1;
    private Vector3 s2;
    private Vector3 s3;
    private Vector3 s4;
    private Vector3 s5;
 
    // Start is called before the first frame update
    private void Start()
    {
        DbButton.onClick.AddListener(Test);
    }

    #region 建立MySql数据库连接

    /// <summary>
    /// 建立数据库连接.
    /// </summary>
    /// <returns>返回MySqlConnection对象</returns>
    private MySqlConnection GetMysqlConnection()
    {
        // string constr = "server=127.0.0.1;port=3308;database=game_schema;user=root;password=j660982";
        string M_str_sqlcon = string.Format("server={0};port={4};user={1};password={2};database={3};", server, userid, password, database, port);
        MySqlConnection myCon = new MySqlConnection(M_str_sqlcon);
        return myCon;
    }

    #endregion 建立MySql数据库连接

    private void Update()
    {
        
    }

    private void Test()
    {
        MySqlConnection mysqlcon = this.GetMysqlConnection();
        mysqlcon.Open();
        try
        {
            bool isOK = mysqlcon.Ping();

            if (isOK)
            {
                Debug.LogError("游戏记录的数据库正常");
            }
            else
            {
                Debug.LogError("游戏记录的数据库错误");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("游戏记录的数据库错误： " + e.Message);
        }
        mysqlcon.Close();
    }

    private void AddData()
    {
        MySqlConnection mysqlcon = this.GetMysqlConnection();
        mysqlcon.Open();
        float xx = player.transform.position.x;
        float yy = player.transform.position.y;
        float zz = player.transform.position.z;
        string sql = "insert into save_game(x,y,z) values ("+xx+","+yy+","+zz+")";
        MySqlCommand cmd = new MySqlCommand(sql, mysqlcon);
        int result = cmd.ExecuteNonQuery();
        Debug.Log("添加成功，第"+result+"条记录");
        mysqlcon.Close();
    }

    private void SearchData(int count)
    {
        MySqlConnection mysqlcon = this.GetMysqlConnection();
        mysqlcon.Open();
        string read_sql = "select * from save_game where id = "+count+"";
        MySqlCommand read_cmd = new MySqlCommand(read_sql, mysqlcon);
        MySqlDataReader reader = read_cmd.ExecuteReader();
        while (reader.Read())
        {
            
            
        }
        
    }
}