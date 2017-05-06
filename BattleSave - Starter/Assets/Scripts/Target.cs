/*
 * Copyright (c) 2017 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
  public int position;

  public GameObject activeRobot;

  [SerializeField]
  private GameObject[] robots;
  [SerializeField]
  private Game game;

  void Start()
  {
    GetComponent<BoxCollider>().enabled = false;

    foreach (GameObject robot in robots)
    {
      robot.SetActive(false);
    }

    if (activeRobot == null)
    {
      StartCoroutine("aliveTimer");
      StartCoroutine("deathTimer");
    }
  }

  public void DisableRobot()
  {
   foreach(GameObject robot in robots)
    {
      robot.GetComponent<Animator>().Play("Base");
      robot.SetActive(false);
    }
    GetComponent<BoxCollider>().enabled = false;
    activeRobot = null;
    StopAllCoroutines();
  }

  // When hit by bullet
  private void OnCollisionEnter(Collision collision)
  {
    Destroy(collision.collider.gameObject);
    activeRobot.GetComponent<Animator>().Play("Die");
    game.AddHit();
    GetComponent<BoxCollider>().enabled = false;
    activeRobot = null;
    StartCoroutine("aliveTimer");
    StartCoroutine("deathTimer");
  }

  public void ActivateRobot()
  {
    activeRobot = robots[Random.Range(0, 3)];
    activeRobot.SetActive(true);
    activeRobot.GetComponent<Animator>().Play("Rise");
    GetComponent<BoxCollider>().enabled = true;
  }

  public void ActivateRobot(RobotTypes type)
  {
    StopAllCoroutines();
    activeRobot = robots[(int)type];
    activeRobot.SetActive(true);
    activeRobot.GetComponent<Animator>().Play("Rise", 0, 1);
    GetComponent<BoxCollider>().enabled = true;
  }

  IEnumerator aliveTimer()
  {
    yield return new WaitForSeconds(Random.Range(2, 6));
    ActivateRobot();
  }

  IEnumerator deathTimer()
  {
    yield return new WaitForSeconds(Random.Range(10, 14));
    if (activeRobot == null)
    {
      yield break;
    }
    activeRobot.GetComponent<Animator>().Play("Die");
    GetComponent<BoxCollider>().enabled = false;
    activeRobot = null;
    StartCoroutine("aliveTimer");
  }

  public void RefreshTimers()
  {
    StopAllCoroutines();
    if (activeRobot == null)
    {
      StartCoroutine("aliveTimer");
      StartCoroutine("deathTimer");
    }
  }
}
