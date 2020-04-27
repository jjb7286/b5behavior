using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;
using RootMotion.FinalIK;


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

    public FullBodyBipedEffector hand;
    public GameObject cube;
    public InteractionObject ikcube;



    private BehaviorAgent behaviorAgent;
	// Use this for initialization
	void Start ()
	{
		behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();
	}

    protected Node PickUp(GameObject p)
    {
        return new Sequence(this.Node_BallStop(),
                            p.GetComponent<BehaviorMecanim>().Node_StartInteraction(hand, ikcube),
                            new LeafWait(1000),
                            p.GetComponent<BehaviorMecanim>().Node_StopInteraction(hand));
    }

    public Node Node_BallStop()
    {
        return new LeafInvoke(() => this.BallStop());
    }
    public virtual RunStatus BallStop()
    {
        Rigidbody rb = cube.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;

        return RunStatus.Success;
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


    protected Node HarryOrient(Transform other)
    {
        return new Sequence(harry.GetComponent<BehaviorMecanim>().Node_OrientTowards(other.position), new LeafWait(1000));
    }

    protected Node TomOrient(Transform other)
    {
        return new Sequence(tom.GetComponent<BehaviorMecanim>().Node_OrientTowards(other.position), new LeafWait(1000));
    }

    protected Node DanielOrient(Transform other)
    {
        return new Sequence(daniel.GetComponent<BehaviorMecanim>().Node_OrientTowards(other.position), new LeafWait(1000));
    }

    protected Node BuildTreeRoot()
	{
        Node roaming = new DecoratorLoop(
                        new Sequence(

                            // all step outside
                            new SequenceParallel(this.ST_ApproachAndWaitDaniel(this.danielsfront), this.ST_ApproachAndWaitTom(this.tomsfront),
                            this.ST_ApproachAndWaitHarry(this.harrysfront)),

                            this.PickUp(harry),

                            // turn and wave
                            new Sequence(this.DanielOrient(harry.transform),
                                        this.HarryOrient(daniel.transform),
                                        new SequenceParallel(daniel.GetComponent<BehaviorMecanim>().Node_BodyAnimation("sit_down_1", true)),
                                        harry.GetComponent<BehaviorMecanim>().Node_BodyAnimation("sit_down_1", true)),


                            // all go to main building, order differing
                            new SequenceShuffle(new Sequence(this.ST_ApproachAndWaitDaniel(this.mainbuildingD),this.DanielOrient(intersection)),
                            new Sequence(this.ST_ApproachAndWaitHarry(this.mainbuildingH), this.HarryOrient(intersection)),
                            new Sequence(this.ST_ApproachAndWaitTom(this.mainbuildingT), this.TomOrient(intersection))),

                            // interact/converse here

                            // all go back to their houses
                            new SequenceParallel(this.ST_ApproachAndWaitDaniel(this.danielshouse), this.ST_ApproachAndWaitTom(this.tomshouse),
                            this.ST_ApproachAndWaitHarry(this.harryshouse))));


                            
                       
        return roaming;
	}
}
