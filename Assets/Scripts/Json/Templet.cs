using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class JsonHandler
{
    readonly string jsonBasePath = Application.persistentDataPath;
    //����� ���
    void Write(Object obj, string path)
    {
        string tmps = JsonUtility.ToJson(obj);
        if (string.IsNullOrEmpty(tmps)) Debug.LogError("JsonWriteError: text is empty!");
        File.WriteAllText(jsonBasePath + "/" + path, tmps);
    }
    T Read<T>(string path) where T : Object
    {
        string tmps = File.ReadAllText(jsonBasePath + "/" + path);
        if (string.IsNullOrEmpty(tmps))
        {
            Debug.LogError("JsonReaderError: text is empty!");
            return null;
        }
        else return JsonUtility.FromJson<T>(tmps);
    }

}
public class CsvHandler
{
    readonly string csvBasePath = Application.persistentDataPath;
    readonly string fileName;
    //��ǲ, �� ��� �񱳴�� �ƿ�ǲ, �ִ���� �����ִ� �񱳴���� ȣ����.
    //2���� �迭�� �����ؾ��Ѵ�.
    //�ʿ��ϴٸ� ������� �и�
    List<List<int>> list = new List<List<int>>();
    public CsvHandler(string fileName)
    {
        this.fileName = fileName;
    }
    public CsvHandler(string basePath, string fileName)
    {
        csvBasePath = basePath;
        this.fileName = fileName;
    }
    public void WriteCsv()
    {
        StreamWriter writer = new StreamWriter(csvBasePath + "/" + fileName);

        writer.WriteLine("1,2,3,5,6,7,8,9,10,12,13,15,16,17,18,19,110,12,13,15,16,17,18,19,110,12,13,15,16,17,18,19,1101,21,31,51,16,17,181,91,11101,121,131,151,161,171,181,191,110");
        writer.WriteLine("1,2,3,5,6,7");
        writer.WriteLine("4");
        writer.Close();
    }
    public void ReadCsv()
    {
        //��Ʈ�� ���� ������ ���پ� �������ִ� ����.
        StreamReader reader = new StreamReader(csvBasePath + "/" + fileName);

        while (!reader.EndOfStream)
            Debug.Log(reader.ReadLine());
        reader.Close();
    }
    public void TestFirstLineSplit()//Ȯ�ΰ�� ���ø��� ���������� ����� �� ���´�.
    {
        StreamReader reader = new StreamReader(csvBasePath + "/" + fileName);

        string[] checkblank = reader.ReadLine().Split(',');
        foreach (var item in checkblank)
        {
            Debug.Log(item);
        }
    }
    public Dictionary<string,Dictionary<string,string>> InstanceReadableDic()
    {
        char divider = '/';
        //csv ��Ʈ�������� ����: ������������ ���� �������� ������ ���� �����ٰ� �ǿ��ٿ��� ĳ�����̸��� �ݵ�� �־�� ������ �� ������.. ���̳� ���� ������� �����ϸ�
        //�� �б� �������� ����ǹǷ� ��������� �Ѵ� ������ ���� �߿��� ���� ���̵� �տ������� �� ����

        StreamReader reader = new StreamReader(csvBasePath + "/" + fileName);
        Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
        int lineCount = -1;
        List<string> getDicKey=new List<string>();
        {
            //ù° ���� ���ø����� �����ؾ��Ѵ�.
            lineCount++;
            string str = reader.ReadLine();
            str.Trim('\n', ' ');
            string[] splitedStr = str.Split(',');
            foreach (var item in splitedStr)
            {
                string strID=item.Split('/')[0];
                getDicKey.Add(strID);//������� ���ԵǴ°��� ���� �� ������ Ű���� �˾Ƴ����̱⶧��.
                dic.Add(strID, new Dictionary<string, string>());//�� ���� ĭ ����
            }
        }
        while (!reader.EndOfStream)
        {
            lineCount++;
            string str = reader.ReadLine();
            str.Trim('\n', ' ');
            string[] splitedStr = str.Split(',');

            //List<int> list = splitedStr.Select(item => int.Parse(item)).ToList();
            //���� ó�� �κ��� ���ο� Ű���ȴ�.
            string first=splitedStr[0].Split(divider)[0];
            //dic[getDicKey[0]].Add(first,first);//Ű����
            for (int i = 1; i < splitedStr.Length; i++)
            {
                string item = splitedStr[i];
                dic[getDicKey[i]].Add(first, item);

                //dic[getDicKey[splitedStr[i]]]
            }
            //list.RemoveAt(0);

            //int[] intar = System.Array.ConvertAll(splitedStr, item => int.Parse(item));
            //int[] cointar = new int[intar.Length - 1];
            //System.Array.Copy(intar,1,cointar,0,cointar.Length);

        }

        reader.Close();

        return dic;
    }

}

public class Templet : MonoBehaviour
{
    private void Start()
    {
        CsvHandler handler = new CsvHandler("testfile.csv");
        //handler.TestFirstLineSplit();
        Dictionary<string,Dictionary<string,string>> instancedDic=handler.InstanceReadableDic();
        print(instancedDic["1"]["3"]);
        print(instancedDic["2"]["4"]);
        //handler.WriteCsv();
        //handler.ReadCsv();
        //handler.InstanceList();
    }
    public static void TestBigO(System.Action action, int trycount, string msg)
    {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        for (int i = 0; i < trycount; i++)
        {
            action();
        }
        stopwatch.Stop();
        Debug.LogError(msg + ": " + stopwatch.ElapsedMilliseconds + "ms");
    }

}