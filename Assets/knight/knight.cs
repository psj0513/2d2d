using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knight : MonoBehaviour
{
    float m_speed = 4.0f;
    float m_rollForce = 6.0f;
    Animator m_animator;
    Rigidbody2D m_body2d;
    bool m_rolling = false;
    int m_facingDirection = 1;
    int m_currentAttack = 0;
    float m_timeSinceAttack = 0.0f;
    float m_delayToIdle = 0.0f;
    float m_rollDuration = 8.0f / 14.0f;
    float m_rollCurrentTime;
    float m_rollexceptTime;
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        m_timeSinceAttack += Time.deltaTime;
        m_rollexceptTime += Time.deltaTime;
        if(m_rolling)
            m_rollCurrentTime += Time.deltaTime;
        
        if(m_rollCurrentTime > m_rollDuration)
            m_rolling = false;
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }
            
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }
        if (!m_rolling )
            m_body2d.velocity = new Vector2(inputX * m_speed, inputY * m_speed);
            
        if(Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling)
        {
            m_currentAttack++;

            if (m_currentAttack > 3)
                m_currentAttack = 1;

            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            m_animator.SetTrigger("Attack" + m_currentAttack);

            m_timeSinceAttack = 0.0f;
        }
        else if (Input.GetKeyDown("left shift") && !m_rolling && m_rollexceptTime>0.7f)
        {
            m_rolling = true;
            m_animator.SetTrigger("Roll");
            m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
            m_rollexceptTime = 0;

        }
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {

            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        else
        {

            m_delayToIdle -= Time.deltaTime;
                if(m_delayToIdle < 0)
                    m_animator.SetInteger("AnimState", 0);
        }
    }
}
