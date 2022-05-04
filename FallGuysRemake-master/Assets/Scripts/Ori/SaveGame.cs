using System;
using System.Data;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MySqlAccess
{
    //连接类对象
    private static MySqlConnection mySqlConnection;
    public static string host = "127.0.0.1";
    public static string userName = "root";
    public static string password = "j660982";
    public static string databaseName = "game_schema";
    public static string port = "3308";
    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="_host">ip地址</param>
    /// <param name="_userName">用户名</param>
    /// <param name="_password">密码</param>
    /// <param name="_databaseName">数据库名称</param>
    public MySqlAccess(string _host,string _port, string _userName, string _password, string _databaseName) {
        host = _host;
        port = _port;
        userName = _userName;
        password = _password;
        databaseName = _databaseName;
        OpenSql();
    }
 
    /// <summary>
    /// 打开数据库
    /// </summary>
    public void OpenSql() {
        try {
            string constr = "server=127.0.0.1;port=3308;database=game_schema;user=root;password=j660982";
            // string M_str_sqlcon = string.Format("server={0};port={4};user={1};password={2};database={3};", host, userName, password, databaseName, port);
            mySqlConnection = new MySqlConnection(constr);
            //if(mySqlConnection.State == ConnectionState.Closed)
            mySqlConnection.Open();
            bool isOK = mySqlConnection.Ping();

            if (isOK)
            {
                Debug.Log("游戏记录的数据库正常");
            }
            else
            {
                Debug.LogError("游戏记录的数据库错误");
            }
            
        } catch (Exception e) {
            Debug.LogError("游戏记录的数据库错误： " + e.Message);
        }
 
    }
 
    /// <summary>
    /// 关闭数据库
    /// </summary>
    public void CloseSql() {
        if (mySqlConnection != null) {
            mySqlConnection.Close();
            mySqlConnection.Dispose();
            mySqlConnection = null;
        }
    }
 
    /// <summary>
    /// 查询数据
    /// </summary>
    /// <param name="tableName">表名</param>
    /// <param name="items">要查询的列</param>
    /// <param name="whereColumnName">查询的条件列</param>
    /// <param name="operation">条件操作符</param>
    /// <param name="value">条件的值</param>
    /// <returns></returns>
    public DataSet Select(string tableName, string[] items, string[] whereColumnName,
        string[] operation, int[] value) {
 
        if (whereColumnName.Length != operation.Length || operation.Length != value.Length) {
            throw new Exception("输入不正确：" + "要查询的条件、条件操作符、条件值 的数量不一致！");
        }
        string query = "Select " + items[0];
        for (int i = 1; i <items.Length; i++) {
            query += "," + items[i];
        }
 
        query += " FROM " + tableName + " WHERE " + whereColumnName[0] + " " + operation[0] + value[0];
        for (int i = 1; i < whereColumnName.Length; i++) {
            query += " and " + whereColumnName[i] + " " + operation[i] + value[i];
        }
        Debug.Log(query);
        return QuerySet(query);
 
    }
    
    // items是更新的列
    public void Update(string tableName,string[] pos,string[] whereColumnName,
        string[] operation, int[] id_value,int[] pos_value)
    {
        if (pos.Length != pos_value.Length) {
            throw new Exception("输入不正确："+"要更改的坐标值数量不等于3");
        }
        if (whereColumnName.Length != operation.Length || operation.Length != id_value.Length) {
            throw new Exception("输入不正确：" + "要查询的条件、条件操作符、条件值 的数量不一致！");
        }
        string query = "UPDATE " + tableName+" SET "+pos[0]+"="+pos_value[0]+","+pos[1]+"="+pos_value[1]+","+pos[2]+"="+pos_value[2];
        
        query +=" WHERE " + whereColumnName[0] + " " + operation[0] + id_value[0];
        for (int i = 1; i < whereColumnName.Length; i++) {
            query += " and " + whereColumnName[i] + " " + operation[i] +  id_value[i];
        }
        Debug.Log(query);
        QuerySet(query);
    }
    /// <summary>
    /// 执行SQL语句
    /// </summary>
    /// <param name="sqlString">sql语句</param>
    /// <returns></returns>
    private DataSet QuerySet(string sqlString) {
        if (mySqlConnection.State == ConnectionState.Open) {
            DataSet ds = new DataSet();
            try {
                MySqlDataAdapter mySqlAdapter = new MySqlDataAdapter(sqlString, mySqlConnection);
                mySqlAdapter.Fill(ds);
            } catch (Exception e) {
                throw new Exception("SQL:" + sqlString + "/n" + e.Message.ToString());
            } finally {
            }
            return ds;
        }
        return null;
    }
}

public class SaveGame : MonoBehaviour, IPointerClickHandler
{
    public GameObject x_position;
    public GameObject y_position;
    public GameObject z_position;
    public Text saveMessage;
 
    //IP地址
    public string host;
    //端口号
    public string port;
    //用户名
    public string userName;
    //密码
    public string password;
    //数据库名称
    public string databaseName;
    //封装好的数据库类
    MySqlAccess mysql;
 
 
    private void Start() {
        saveMessage = GameObject.FindGameObjectWithTag("SaveMessage").GetComponent<Text>();
        mysql = new MySqlAccess(host, port, userName, password, databaseName);
    }
 
    
    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.pointerPress.name == "load1")
        {
            OnClickedLoadButton(1);
        }
        else if (eventData.pointerPress.name == "load2")
        {
            OnClickedLoadButton(2);
        }
        else if (eventData.pointerPress.name == "load3")
        {
            OnClickedLoadButton(3);
        }
        else if (eventData.pointerPress.name == "save1")
        {
            OnClickedSaveButton(1);
        }
        else if (eventData.pointerPress.name == "save2")
        {
            OnClickedSaveButton(2);
        }
        else if (eventData.pointerPress.name == "save3")
        {
            OnClickedSaveButton(3);
        }

    }

    private void OnClickedLoadButton(int i)
    {
        mysql.OpenSql();
        string loadMsg = "";
        DataSet ds = mysql.Select("save_game", new string[] {"*"},
            new string[] {"id"}, new string[] {"="}, new int[] {i});
        if (ds != null)
        {
            DataTable table = ds.Tables[0];
            if (table.Rows.Count > 0) {
                loadMsg = "存档"+i+"载入成功！";
                saveMessage.color = Color.green;
                GameObject.Find("x_position").GetComponent<GameData>().param = Convert.ToInt16(table.Rows[0][1]);
                GameObject.Find("y_position").GetComponent<GameData>().param = Convert.ToInt16(table.Rows[0][2]);
                GameObject.Find("z_position").GetComponent<GameData>().param = Convert.ToInt16(table.Rows[0][3]);
                Debug.Log(table.Rows[0][1]+","+table.Rows[0][2]+","+table.Rows[0][3]);
                GameObject.Find("flag_position").GetComponent<GameData>().param = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            } else {
                loadMsg = "存档"+i+"损坏！";
                saveMessage.color = Color.red;
            }
            saveMessage.text = loadMsg;
        }
        else
        {
            Debug.Log("当前位置暂无存档");
        }
        mysql.CloseSql();
    }

    private void OnClickedSaveButton(int i)
    {
        mysql.OpenSql();
        string saveMsg = "";
        mysql.Update("save_game", new string[] {"x", "y", "z"},
            new string[] {"id"}, new string[] {"="}, new int[] {i},
            new int[]{Convert.ToInt16(GameObject.Find("FallGuysPlayer1").transform.position.x),Convert.ToInt16(GameObject.Find("FallGuysPlayer1").transform.position.y),Convert.ToInt16(GameObject.Find("FallGuysPlayer1").transform.position.z)});
        Debug.Log("已保存至存档"+i);
        mysql.CloseSql();
    }
}