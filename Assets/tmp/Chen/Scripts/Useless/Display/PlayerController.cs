using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int m_speed;
    private IBattleActorEntity m_battleActor;
    public IBattleActorEntity Entity { get { return m_battleActor; }set { m_battleActor = value; } }


    void Start()
    {
        m_speed = m_battleActor.EntitySpeedGet();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            OptCmd optCmd = new OptCmd();
            optCmd.m_moveDirectionEnum = MoveDirectionEnum.FRONT;
            optCmd.m_optCmdType = OptCmdType.Movement;
            m_battleActor.OptCmdInput(optCmd);

        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            OptCmd optCmd = new OptCmd();
            optCmd.m_moveDirectionEnum = MoveDirectionEnum.LEFT;
            optCmd.m_optCmdType = OptCmdType.Movement;
            m_battleActor.OptCmdInput(optCmd);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            OptCmd optCmd = new OptCmd();
            optCmd.m_moveDirectionEnum = MoveDirectionEnum.BACK;
            optCmd.m_optCmdType = OptCmdType.Movement;
            m_battleActor.OptCmdInput(optCmd);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            OptCmd optCmd = new OptCmd();
            optCmd.m_moveDirectionEnum = MoveDirectionEnum.RIGHT;
            optCmd.m_optCmdType = OptCmdType.Movement;
            m_battleActor.OptCmdInput(optCmd);
        }
    }
}
