using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class BehaviorTree : MonoBehaviour
{
	public Transform intersection;
	public Transform mainbuilding;
	public Transform topcorner;
    public Transform bottomcorner;
    public Transform danielsfront;
    public Transform danielshouse;
    public Transform harrysfront;
    public Transform harryshouse;
    public Transform tomsfront;
    public Transform tomshouse;
    public GameObject daniel;
    public GameObject harry;
    public GameObject tom;


    private BehaviorAgent behaviorAgent;
	// Use this for initialization
	void Start ()
	{
		behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	protected Node ST_ApproachAndWaitDaniel(Transform target)
	{
		Val<Vector3> position = Val.V (() => target.position);
		return new Sequence(daniel.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(500));
	}

    protected Node ST_ApproachAndWaitHarry(Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(harry.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(500));
    }

    protected Node ST_ApproachAndWaitTom(Transform target)
        {
            Val<Vector3> position = Val.V(() => target.position);
            return new Sequence(tom.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(500));
        }

    protected Node BuildTreeRoot()
	{
        Node roaming = new DecoratorLoop(
                        new Sequence(
                            new SequenceParallel(this.ST_ApproachAndWaitDaniel(this.danielsfront),
                            this.ST_ApproachAndWaitHarry(this.harrysfront), this.ST_ApproachAndWaitTom(this.tomsfront)),

                            new SequenceParallel(this.ST_ApproachAndWaitDaniel(this.mainbuilding),
                            this.ST_ApproachAndWaitHarry(this.mainbuilding), this.ST_ApproachAndWaitTom(this.mainbuilding))));

                        //this.ST_ApproachAndWaitDaniel(this.bottomcorner),
                        //this.ST_ApproachAndWaitHarry(this.intersection),


                        //this.ST_ApproachAndWaitTom(this.topcorner),
                       
        return roaming;
	}
}
