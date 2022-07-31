using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;

public class ProtocolTool
{
    //�����ļ�����·��
    private static string PROTO_INFO_PATH = Application.dataPath + "/Editor/ProtocolTool/ProtocolInfo.xml";

    private static GenerateCSharp generateCSharp = new GenerateCSharp();

    [MenuItem("ProtocolTool/生成C#协议代码")]
    private static void GenerateCSharp()
    {
        //1.��ȡxml��ص���Ϣ
        //XmlNodeList list = GetNodes("enum");
        //2.������Щ��Ϣ ȥƴ���ַ��� ���ɶ�Ӧ�Ľű�
        //���ɶ�Ӧ��ö�ٽű�
        generateCSharp.GenerateEnum(GetNodes("enum"));
        //���ɶ�Ӧ�����ݽṹ��ű�
        generateCSharp.GenerateData(GetNodes("data"));
        //���ɶ�Ӧ����Ϣ��ű�
        generateCSharp.GenerateMsg(GetNodes("message"));
        //������Ϣ��
        generateCSharp.GenerateMsgPool(GetNodes("message"));

        //ˢ�±༭������ �����ǿ��Կ������ɵ����� ����Ҫ�ֶ�����ˢ����
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// ��ȡָ�����ֵ������ӽڵ� �� List
    /// </summary>
    /// <param name="nodeName"></param>
    /// <returns></returns>
    private static XmlNodeList GetNodes(string nodeName)
    {
        XmlDocument xml = new XmlDocument();
        xml.Load(PROTO_INFO_PATH);
        XmlNode root = xml.SelectSingleNode("messages");
        return root.SelectNodes(nodeName);
    }
}
