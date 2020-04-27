using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;


public class BehaviorTree : MonoBehaviour
{
	public Transform intersection;
	public Transform mainbuildingD;
    public Transform mainbuildingT;
    public Transform mainbuildingH;
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
    public bool meetup;



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

    //protected Node HarryFinds()
    //{
    //return new Sequence(
    //harry.GetComponent<BehaviorMecanim>().Node_FaceAnimation("acknowledging",true));
    //}

    protected Node HarryOrient()
    {
        return new Sequence(harry.GetComponent<BehaviorMecanim>().Node_OrientTowards(intersection.position), new LeafWait(1000));
    }

    protected Node TomOrient()
    {
        return new Sequence(tom.GetComponent<BehaviorMecanim>().Node_OrientTowards(intersection.position), new LeafWait(1000));
    }

    protected Node DanielOrient()
    {
        return new Sequence(daniel.GetComponent<BehaviorMecanim>().Node_OrientTowards(intersection.position), new LeafWait(1000));
    }

    protected Node BuildTreeRoot()
	{
        Node roaming = new DecoratorLoop(
                        new Sequence(

                            // all step outside
                            new SequenceParallel(this.ST_ApproachAndWaitDaniel(this.danielsfront), this.ST_ApproachAndWaitTom(this.tomsfront),
                            this.ST_ApproachAndWaitHarry(this.harrysfront)),

                            // harry finds wrong package
                            //this.HarryFinds(),

                            // all go to main building, order differing
                            new SequenceShuffle(new Sequence(this.ST_ApproachAndWaitDaniel(this.mainbuildingD),this.DanielOrient()),
                            new Sequence(this.ST_ApproachAndWaitHarry(this.mainbuildingH), this.HarryOrient()),
                            new Sequence(this.ST_ApproachAndWaitTom(this.mainbuildingT), this.TomOrient())),

                            // interact/converse here

                            // all go back to their houses
                            new SequenceParallel(this.ST_ApproachAndWaitDaniel(this.danielshouse), this.ST_ApproachAndWaitTom(this.tomshouse),
                            this.ST_ApproachAndWaitHarry(this.harryshouse))));


                            
                       
        return roaming;
	}
}
