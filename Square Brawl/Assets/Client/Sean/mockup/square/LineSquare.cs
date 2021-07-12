using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineSquare : Graphic
{
    public float m_Thickness = 10;
    public float m_Size;
    
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        vertex.position = new Vector3(-m_Size/2, -m_Size / 2);
        vh.AddVert(vertex);

        vertex.position = new Vector3(-m_Size / 2, m_Size/2);
        vh.AddVert(vertex);

        vertex.position = new Vector3(m_Size/2, m_Size/2);
        vh.AddVert(vertex);

        vertex.position = new Vector3(m_Size/2, -m_Size / 2);
        vh.AddVert(vertex);

        float widthSqr = m_Thickness * m_Thickness;
        float distanceSqr = widthSqr / 2f;
        float distance = Mathf.Sqrt(distanceSqr);

        vertex.position = new Vector3(-m_Size / 2 + distance, -m_Size / 2 +  distance);
        vh.AddVert(vertex);

        vertex.position = new Vector3(-m_Size / 2 + distance, m_Size/2 - distance);
        vh.AddVert(vertex);

        vertex.position = new Vector3(m_Size/2 - distance, m_Size/2 - distance);
        vh.AddVert(vertex);

        vertex.position = new Vector3(m_Size/2 - distance, -m_Size / 2 + distance);
        vh.AddVert(vertex);

        vh.AddTriangle(0, 1, 5);
        vh.AddTriangle(5, 4, 0);

        vh.AddTriangle(1, 2, 6);
        vh.AddTriangle(6, 5, 1);

        vh.AddTriangle(2, 3, 7);
        vh.AddTriangle(7, 6, 2);

        vh.AddTriangle(3, 0, 4);
        vh.AddTriangle(4, 7, 3);
    }
}
