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
    public TextMesh textObject;
    public BodyMecanim Body;

    private BehaviorAgent behaviorAgent;
	// Use this for initialization
	void Start ()
	{
		behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();
       TextMesh textObject = GameObject.Find("narrator").GetComponent<TextMesh>();
       textObject.text="...";
	}



    // Update is called once per frame
    void Update ()
	{
    

	}

	protected Node ST_ApproachAndWaitDaniel(Transform target)
	{
		Val<Vector3> position = Val.V (() => target.position);
		return new Sequence(daniel.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(5));
        
	}

    protected Node ST_ApproachAndWaitHarry(Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(harry.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(5));
    }

    protected Node ST_ApproachAndWaitTom(Transform target)
        {
            
            Val<Vector3> position = Val.V(() => target.position);
            if(tom.transform.position>13){
            textObject.text="Tom's here!";
            }
            return new Sequence(tom.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(5));
        }
    
    

    protected Node BuildTreeRoot()
	{
        Node roaming = new DecoratorLoop(
                        new Sequence(
                            
                            // all step outside
                            new SequenceParallel(this.ST_ApproachAndWaitDaniel(this.danielsfront), this.ST_ApproachAndWaitTom(this.tomsfront),
                            this.ST_ApproachAndWaitHarry(this.harrysfront)),

                            // all go to main building, order differing
                            new SequenceShuffle(this.ST_ApproachAndWaitDaniel(this.mainbuildingD), this.ST_ApproachAndWaitHarry(this.mainbuildingH),
                            this.ST_ApproachAndWaitTom(this.mainbuildingT)),

                            // interaction here
                            
                            // all go back to their houses
                            new SequenceParallel(this.ST_ApproachAndWaitDaniel(this.danielshouse),this.ST_ApproachAndWaitTom(this.tomshouse),
                            this.ST_ApproachAndWaitHarry(this.harryshouse))));


                            
                       
        return roaming;
	}
}
