using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    public class SynthManager : Singleton<SynthManager>
    {

        public float _ChaosMaxRange = 0.2f;

        //Add ref for each element in the synth
        public DreamStarAnimator _Animator;
        public DreamStarGenerator_Mixer _Mixer;

        public Circle _Circle1;
        public Circle _Circle2;

        public Curved _Curved1;
        public Curved _Curved2;
        public Curved _Curved3;

        //method to add a random amount to a field 



        //maybe add a ref to each material preset 

        public void RandomUpgrade()
        {
            int r = Random.Range(1, 2);
            Debug.Log("synth upgrade: " + r);
            switch(r)
            {
                case 1: WholeCircle();
                    break;
                case 2: WholeCurve();//FractCircle();
                    break;
                case 3: //ChaosCircle();
                    break;
                case 4: //WholeCurve();
                    break;
                case 5: //FractCurve();
                    break;
                case 6: //ChaosCurve();
                    break;
            }
        }
        
        //---------------------------------------------------------------------------Functions To Call---------------------------------------------------------------
        public void WholeCircle()
        {
            //bool Rand = GetRandomBoolean();
            //if(Rand)
             AddToCircle(_Circle1, 0,  1);
            //else            
            //    AddToCircle(_Circle1, 0, -1);            
        } 

        public void FractCircle()
        {
            //bool Rand = GetRandomBoolean();
            //if(Rand)
                AddToCircle(_Circle2, 0,  0.1f);
            //else            
                //AddToCircle(_Circle2, 0, -0.1f);            
        }

        public void ChaosCircle()
        {
            float fl = GetRandomFloat();
            //bool bl = GetRandomBoolean();

            //if (bl)
                AddToCircle(_Circle1, 0, fl);
            //else
            //    AddToCircle(_Circle2, 0, fl);
        }

        public void WholeCurve()
        {
            //bool Rand = GetRandomBoolean();
            //if (Rand)
                AddToCurve(_Curved1, 0, 1);
            //else
            //    AddToCurve(_Curved1, 0, -0.5f);
        }

        public void FractCurve()
        {
            //bool Rand = GetRandomBoolean();
            //if (Rand)
               AddToCurve(_Curved2, 0, 0.1f);

            //else
            //    AddToCurve(_Curved2, 0, -0.1f);
        }

        public void ChaosCurve()
        {
            float fl = GetRandomFloat();
            AddToCurve(_Curved3, 0, fl);
        }
        //-----------------------------------------------------------------------------------------------------------------------


        public void AddToCircle(Circle c, float impact, float angle)
        {
            c.Impact += impact;
            c.Angle_MP += angle;
        }

        public void AddToCurve(Curved c, float impact, float angle)
        {
            c.Impact += impact;
            c.Angle_MP += angle;
        }

        bool GetRandomBoolean()
        {
            int randomInt = Random.Range(0, 2);
            return randomInt == 1;
        }

        float GetRandomFloat()
        {
            return Random.Range(-_ChaosMaxRange, _ChaosMaxRange);
        }
    }
