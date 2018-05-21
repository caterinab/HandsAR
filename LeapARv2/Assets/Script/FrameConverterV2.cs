// ===============================
// AUTHOR     : Mirko Pani
// CREATE DATE     : 24/10/17
// PURPOSE     : Questa classe converte una stringa LEAP MOTION JSon v6 in un Frame valido
// SPECIAL NOTES: SDK Orion v3.2 e leap json v6
// ===============================
// Change History:
//
//==================================
/*
using CustomLeap;
using Leap;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LeapFrame
{
    public float currentFrameRate;
    public long id;
    public IList<IList<float>> r;
    public float s;
    public IList<float> t;
    public int timestamp;
    public IList<int> devices;
    public IList<Gestures> gestures;
    public IList<Hands> hands;
    public InteractionBoxObject interactionBox;
    public IList<Pointables> pointables;
}

public class Gestures
{
    public IList<float> center;
    public IList<float> direction;
    public int duration;
    public IList<int> handsIds;
    public int id;
    public IList<float> normal;
    public int pointableIds;
    public IList<float> position;
    public float progress;
    public float radius;
    public float speed;
    public IList<float> startPosition;
    public string state;
    public string type;
}

public class Hands
{
    public IList<IList<float>> armBasis;
    public float armWidth;
    public float confidence;
    public IList<float> direction;
    public IList<float> elbow;
    public float grabAngle;
    public float grabStrength;
    public int id;
    public IList<float> palmNormal;
    public IList<float> palmPosition;
    public IList<float> palmVelocity;
    public float palmWidth;
    public float pinchDistance;
    public float pinchStrength;
    public IList<IList<float>> r;
    public float s;
    public IList<float> sphereCenter;
    public float sphereRadius;
    public IList<float> stabilizedPalmPosition;
    public IList<float> t;
    public float timeVisible;
    public string type;
    public IList<float> wrist;
}

public class InteractionBoxObject
{
    public IList<float> center;
    public IList<float> size;
}


public class Pointables
{
    public IList<float> bases;
    public IList<float> btipPosition;
    public Vector carpPosition;
    public IList<float> dipPosition;
    public Vector direction;
    public bool extended;
    public int handId;
    public int id;
    public float length;
    public Vector mcpPosition;
    public IList<float> pipPosition;
    public Vector stabilizedTipPosition;
    public float timeVisible;
    public Vector tipPosition;
    public Vector tipVelocity;
    public bool tool;
    public float touchDistance;
    public string touchZone;
    public int type;
    public float width;
}

public class FrameConverterV2
{
    float[,] bases = new float[4, 9];
    float[] btipPosition = new float[3];
    float[] carpPosition = new float[3];
    float[] dipPosition = new float[3];
    float[] direction = new float[3];
    bool extended;
    int handId;
    int id;
    float length;
    float[] mcpPosition = new float[3];
    float[] pipPosition = new float[3];
    float[] stabilizedTipPosition = new float[3];
    float timeVisible;
    float[] tipPosition = new float[3];
    float[] tipVelocity = new float[3];
    int type;
    float width;
    Vector tipPositionV, tipVelocityV, carpPositionV, mcpPositionV, prev, next;
    Bone distal, metacarpal, proximal, intermediate;
    bool isLeft = false;
    int indiceMano = -1;
    Leap.Finger finger;
    Vector x, y, z;
    LeapQuaternion qua;
    Vector dipPositionV;


    /// <summary>
    /// Converte una stringa LEAP MOTION JSon v6 in un Frame valido
    /// </summary>
    /// <param name="param1">Stringa json v6 valida</param>
    /// <returns>Frame valido</returns>
    public Frame ConvertFromString(string jsonString)
    {
        var reader = new JsonTextReader(new StringReader(jsonString));
        LeapFrame i = JsonConvert.DeserializeObject<LeapFrame>(jsonString);
        
        foreach (Hands h in i.hands)
        {
            List<Finger> fingers;
            Finger f;

            foreach (Pointables p in i.pointables)
            {
                if (p.handId == h.id)
                {
                    f = new Finger(i.id, h.id, p.id, p.timeVisible, p.tipPosition, p.tipVelocity,
                        p.direction, p.stabilizedTipPosition, p.width, p.length, p.extended, (Finger.FingerType)p.type,
                        new Bone(p.carpPosition, p.mcpPosition, )
                }
            }

            bool isLeft = false;

            if (h.type == "left")
            {
                isLeft = true;
            }

            Hand hand = new Hand(i.id, h.id, h.confidence, h.grabStrength, h.grabAngle, h.pinchStrength, h.pinchDistance, h.palmWidth, isLeft, h.timeVisible, null, )
        }

        Frame frame = new Leap.Frame(i.id, i.currentFrameRate, i.interactionBox, i.hands);
        
        if (reader.TokenType == JsonToken.Integer && currentProperty == "timestamp")
            frame.Timestamp = long.Parse(reader.Value.ToString());

        if (reader.TokenType == JsonToken.String && currentProperty == "currentFrameRate")
            frame.CurrentFramesPerSecond = float.Parse(reader.Value.ToString());

        if (reader.TokenType == JsonToken.PropertyName && currentProperty == "interactionBox")
        {
            float[] center = new float[3];
            float[] size = new float[3];
            reader.Read();
            reader.Read();
            reader.Read();
            //center
            reader.Read();
            center[0] = float.Parse(reader.Value.ToString());
            reader.Read();
            center[1] = float.Parse(reader.Value.ToString());
            reader.Read();
            center[2] = float.Parse(reader.Value.ToString());
            reader.Read();
            reader.Read();
            reader.Read();
            //size
            reader.Read();
            size[0] = float.Parse(reader.Value.ToString());
            reader.Read();
            size[1] = float.Parse(reader.Value.ToString());
            reader.Read();
            size[2] = float.Parse(reader.Value.ToString());
            reader.Read();
            reader.Read();

            //Creo interactionBox
            frame.InteractionBox = new Leap.InteractionBox(new Vector(center[0], center[1], center[2])
, new Vector(size[0], size[1], size[2]));

        }

        if (reader.TokenType == JsonToken.PropertyName && currentProperty == "hands")
        {
            //StartArray
            reader.Read();
            float[] armBasis = new float[9];
            float armWidth;
            float confidence;
            float[] direction = new float[3];
            float[] elbow = new float[3];
            int id;
            float grabAngle;
            float grabStrength;
            float[] palmNormal = new float[3];
            float[] palmPosition = new float[3];
            float[] palmVelocity = new float[3];
            float palmWidth;
            float pinchDistance;
            float[] stabilizedPalmPosition = new float[3];
            float[] wrist = new float[3];
            float pinchStrength;
            float timeVisible;
            bool isLeft;

            //Per ogni mano entra dentro, altrimenti legge endarray
            while (reader.Read() && reader.TokenType == JsonToken.StartObject)
            {


                #region Parsing Mano
                //propertyname armbasis
                reader.Read();
                //start array
                reader.Read();
                //start array
                reader.Read();
                for (int i = 0; i < 9; i++)
                {
                    reader.Read();
                    armBasis[i] = float.Parse(reader.Value.ToString());
                    reader.Read();
                    armBasis[++i] = float.Parse(reader.Value.ToString());
                    reader.Read();
                    armBasis[++i] = float.Parse(reader.Value.ToString());
                    //EndArray
                    reader.Read();
                    reader.Read();
                }
                //EndArray

                //propertyname armwidth
                reader.Read();
                reader.Read();
                armWidth = float.Parse(reader.Value.ToString());
                //Confidence
                reader.Read();
                reader.Read();
                confidence = float.Parse(reader.Value.ToString());
                //Direction
                reader.Read();
                //startArray
                reader.Read();

                reader.Read();
                direction[0] = float.Parse(reader.Value.ToString());
                reader.Read();
                direction[1] = float.Parse(reader.Value.ToString());
                reader.Read();
                direction[2] = float.Parse(reader.Value.ToString());
                //Token: EndArray
                //Token: PropertyName, Value: elbow
                //Token: StartArray
                
                reader.Read();
                reader.Read();
                reader.Read();

                reader.Read();
                elbow[0] = float.Parse(reader.Value.ToString());
                reader.Read();
                elbow[1] = float.Parse(reader.Value.ToString());
                reader.Read();
                elbow[2] = float.Parse(reader.Value.ToString());

                reader.Read();
                reader.Read();
                //grabangle value
                reader.Read();
                grabAngle = float.Parse(reader.Value.ToString());
                //Grabstrength
                reader.Read();
                reader.Read();
                grabStrength = float.Parse(reader.Value.ToString());
                //id
                reader.Read();
                reader.Read();
                id = int.Parse(reader.Value.ToString());
                //Palmnormal
                reader.Read();
                reader.Read();

                reader.Read();
                palmNormal[0] = float.Parse(reader.Value.ToString());
                reader.Read();
                palmNormal[1] = float.Parse(reader.Value.ToString());
                reader.Read();
                palmNormal[2] = float.Parse(reader.Value.ToString());

                reader.Read();
                reader.Read();
                reader.Read();
                //palmposition
                reader.Read();
                palmPosition[0] = float.Parse(reader.Value.ToString());
                reader.Read();
                palmPosition[1] = float.Parse(reader.Value.ToString());
                reader.Read();
                palmPosition[2] = float.Parse(reader.Value.ToString());

                reader.Read();
                reader.Read();
                reader.Read();
                //palmvelocity
                reader.Read();
                palmVelocity[0] = float.Parse(reader.Value.ToString());
                reader.Read();
                palmVelocity[1] = float.Parse(reader.Value.ToString());
                reader.Read();
                palmVelocity[2] = float.Parse(reader.Value.ToString());

                reader.Read();
                reader.Read();
                //palmwidth
                reader.Read();
                palmWidth = float.Parse(reader.Value.ToString());
                reader.Read();
                reader.Read();
                pinchDistance = float.Parse(reader.Value.ToString());
                reader.Read();
                reader.Read();
                pinchStrength = float.Parse(reader.Value.ToString());

                //Ignoro r, sphereradius e spherecenter
                for (int i = 0; i < 28; i++)
                {
                    reader.Read();
                }
                //stabilized
                reader.Read();
                reader.Read();
                reader.Read();
                stabilizedPalmPosition[0] = float.Parse(reader.Value.ToString());
                reader.Read();
                stabilizedPalmPosition[1] = float.Parse(reader.Value.ToString());
                reader.Read();
                stabilizedPalmPosition[2] = float.Parse(reader.Value.ToString());
                reader.Read();
                //Ignoro t
                for (int i = 0; i < 6; i++)
                {
                    reader.Read();
                }
                //timevisible
                reader.Read();
                reader.Read();
                timeVisible = float.Parse(reader.Value.ToString());
                //Type
                reader.Read();
                reader.Read();
                isLeft = (reader.Value.ToString() == "left") ? true : false;
                //wrist
                reader.Read();
                reader.Read();

                reader.Read();
                wrist[0] = float.Parse(reader.Value.ToString());
                reader.Read();
                wrist[1] = float.Parse(reader.Value.ToString());
                reader.Read();
                wrist[2] = float.Parse(reader.Value.ToString());

                reader.Read();
                //Fine object mano
                reader.Read();
                #endregion

                //creiamo mano
                #region Creazione Mano
                Vector directionVector = new Leap.Vector(direction[0], direction[1], direction[2]);
                Vector palmNormalVector = new Leap.Vector(palmNormal[0], palmNormal[1], palmNormal[2]);

                Leap.Hand rHand = new Leap.Hand();
                rHand.FrameId = frame.Id;
                rHand.Id = id;
                rHand.Confidence = confidence;
                rHand.GrabStrength = grabStrength;
                rHand.GrabAngle = grabAngle;
                rHand.PinchStrength = pinchStrength;
                rHand.PinchDistance = pinchDistance;
                rHand.PalmWidth = palmWidth;
                rHand.IsLeft = isLeft;
                rHand.TimeVisible = timeVisible;
                rHand.PalmPosition = new Leap.Vector(palmPosition[0], palmPosition[1], palmPosition[2]);
                rHand.StabilizedPalmPosition = new Leap.Vector(stabilizedPalmPosition[0], stabilizedPalmPosition[1], stabilizedPalmPosition[2]);

                Debug.Log("Palm and stabilized palm positions: " + rHand.PalmPosition + " " + rHand.StabilizedPalmPosition + "\n");

                rHand.PalmVelocity = new Leap.Vector(palmVelocity[0], palmVelocity[1], palmVelocity[2]);
                rHand.PalmNormal = palmNormalVector;
                rHand.Direction = directionVector;
                rHand.WristPosition = new Leap.Vector(wrist[0], wrist[1], wrist[2]);

                //Calcoliamo rotation (LeapQuaternion)
                Vector x = directionVector.Cross(palmNormalVector) * -1;
                rHand.Rotation = QuaternionHelper.generateQuaternion(x, palmNormalVector * -1, directionVector * -1);

                if (armBasis.Length > 0)
                {
                    //arm
                    //ref: https://github.com/logotype/LeapMotionAS3/blob/adca9465d6209a6b98073c9fc50660a05c9a5579/src/com/leapmotion/leap/socket/LeapSocket.as

                    Leap.Vector elbowV = new Leap.Vector(elbow[0], elbow[1], elbow[2]);
                    Leap.Vector wristV = new Leap.Vector(wrist[0], wrist[1], wrist[2]);
                    Leap.Vector diff = elbowV - wristV;

                    Leap.LeapQuaternion rotQuat = rHand.IsLeft ? QuaternionHelper.generateQuaternion(new Vector(-armBasis[0], -armBasis[1], -armBasis[2]), new Vector(armBasis[3], armBasis[4], armBasis[5]), new Vector(armBasis[6], armBasis[7], armBasis[8])) : QuaternionHelper.generateQuaternion(new Vector(armBasis[0], armBasis[1], armBasis[2]), new Vector(armBasis[3], armBasis[4], armBasis[5]), new Vector(armBasis[6], armBasis[7], armBasis[8]));



                    rHand.Arm = new Leap.Arm(
                                elbowV,
                                wristV,
                                elbowV - wristV,
                                new Leap.Vector(armBasis[6] * -1, armBasis[7] * -1, armBasis[8] * -1),
                                diff.Magnitude,
                                armWidth,
                                rotQuat);
                }
                else
                {
                    rHand.Arm = new Leap.Arm();
                }

                //fingers
                rHand.Fingers = new List<Leap.Finger>();

                #endregion
                frame.Hands.Add(rHand);
            }
        }


        if (reader.TokenType == JsonToken.PropertyName && currentProperty == "pointables")
        {
            //StartArray
            reader.Read();



            //Per ogni pointable entra dentro, altrimenti legge endarray
            while (reader.Read() && reader.TokenType == JsonToken.StartObject)
            {


                #region parsing dito
                //bases
                reader.Read();
                reader.Read();
                reader.Read();

                //Tutte le falangi per un dito
                for (int j = 0; j < 4; j++)
                {
                    //Singola falange
                    for (int i = 0; i < 9; i += 3)
                    {
                        //startarray
                        reader.Read();
                        reader.Read();
                        bases[j, i] = float.Parse(reader.Value.ToString());
                        reader.Read();
                        bases[j, i + 1] = float.Parse(reader.Value.ToString());
                        reader.Read();
                        bases[j, i + 2] = float.Parse(reader.Value.ToString());
                        //endarray
                        reader.Read();
                    }
                    //endarray
                    reader.Read();
                    //startarray o endarray finale
                    reader.Read();
                }
                reader.Read();
                //btip position
                reader.Read();
                reader.Read();
                btipPosition[0] = float.Parse(reader.Value.ToString());
                reader.Read();
                btipPosition[1] = float.Parse(reader.Value.ToString());
                reader.Read();
                btipPosition[2] = float.Parse(reader.Value.ToString());
                reader.Read();
                reader.Read();
                reader.Read();

                reader.Read();
                carpPosition[0] = float.Parse(reader.Value.ToString());
                reader.Read();
                carpPosition[1] = float.Parse(reader.Value.ToString());
                reader.Read();
                carpPosition[2] = float.Parse(reader.Value.ToString());
                reader.Read();
                reader.Read();
                reader.Read();

                reader.Read();
                dipPosition[0] = float.Parse(reader.Value.ToString());
                reader.Read();
                dipPosition[1] = float.Parse(reader.Value.ToString());
                reader.Read();
                dipPosition[2] = float.Parse(reader.Value.ToString());
                reader.Read();
                reader.Read();
                reader.Read();

                reader.Read();
                direction[0] = float.Parse(reader.Value.ToString());
                reader.Read();
                direction[1] = float.Parse(reader.Value.ToString());
                reader.Read();
                direction[2] = float.Parse(reader.Value.ToString());
                reader.Read();
                reader.Read();

                //Extended
                reader.Read();
                extended = bool.Parse(reader.Value.ToString());

                reader.Read();
                reader.Read();
                handId = int.Parse(reader.Value.ToString());

                reader.Read();
                reader.Read();
                id = int.Parse(reader.Value.ToString());

                //length
                reader.Read();
                reader.Read();
                length = float.Parse(reader.Value.ToString());

                //mcpposition
                reader.Read();
                reader.Read();

                reader.Read();
                mcpPosition[0] = float.Parse(reader.Value.ToString());
                reader.Read();
                mcpPosition[1] = float.Parse(reader.Value.ToString());
                reader.Read();
                mcpPosition[2] = float.Parse(reader.Value.ToString());

                reader.Read();
                reader.Read();
                reader.Read();

                reader.Read();
                pipPosition[0] = float.Parse(reader.Value.ToString());
                reader.Read();
                pipPosition[1] = float.Parse(reader.Value.ToString());
                reader.Read();
                pipPosition[2] = float.Parse(reader.Value.ToString());

                reader.Read();
                reader.Read();
                reader.Read();

                reader.Read();
                stabilizedTipPosition[0] = float.Parse(reader.Value.ToString());
                reader.Read();
                stabilizedTipPosition[1] = float.Parse(reader.Value.ToString());
                reader.Read();
                stabilizedTipPosition[2] = float.Parse(reader.Value.ToString());

                reader.Read();
                reader.Read();

                reader.Read();
                timeVisible = float.Parse(reader.Value.ToString());

                reader.Read();
                reader.Read();

                reader.Read();
                tipPosition[0] = float.Parse(reader.Value.ToString());
                reader.Read();
                tipPosition[1] = float.Parse(reader.Value.ToString());
                reader.Read();
                tipPosition[2] = float.Parse(reader.Value.ToString());


                reader.Read();
                reader.Read();
                reader.Read();

                reader.Read();
                tipVelocity[0] = float.Parse(reader.Value.ToString());
                reader.Read();
                tipVelocity[1] = float.Parse(reader.Value.ToString());
                reader.Read();
                tipVelocity[2] = float.Parse(reader.Value.ToString());

                //Informazioni inutili
                for (int i = 0; i < 7; i++)
                    reader.Read();
                reader.Read();

                reader.Read();
                type = int.Parse(reader.Value.ToString());

                reader.Read();
                reader.Read();
                width = float.Parse(reader.Value.ToString());

                reader.Read();
                #endregion

                #region creazione dito

                //Otteniamo isleft e indice
                for (int j = 0; j < frame.Hands.Count; j++)
                {
                    if (frame.Hands[j].Id == handId)
                    {
                        isLeft = frame.Hands[j].IsLeft;
                        indiceMano = j;
                        break;
                    }
                }


                tipPositionV = new Vector(tipPosition[0], tipPosition[1], tipPosition[2]);
                tipVelocityV = new Vector(tipVelocity[0], tipVelocity[1], tipVelocity[2]);
                carpPositionV = new Vector(carpPosition[0], carpPosition[1], carpPosition[2]);
                mcpPositionV = new Vector(mcpPosition[0], mcpPosition[1], mcpPosition[2]);
                dipPositionV = new Vector(dipPosition[0], dipPosition[1], dipPosition[2]);

                prev = carpPositionV;
                next = mcpPositionV;

                Vector directionV = next - prev;

                if (length < float.Epsilon)
                {
                    directionV = Vector.Zero;
                }
                else
                {
                    directionV /= length;
                }

                //Rotation matrix
                x = new Vector(bases[0, 0], bases[0, 1], bases[0, 2]);
                y = new Vector(bases[0, 3], bases[0, 4], bases[0, 5]);
                z = new Vector(bases[0, 6], bases[0, 7], bases[0, 8]);


                if (isLeft)
                    qua = QuaternionHelper.generateQuaternion(-x, y, z);
                else
                    qua = QuaternionHelper.generateQuaternion(x, y, z);

                metacarpal = new Bone(
                  carpPositionV,
                   mcpPositionV,
                   (prev + next) / 2.0f,
                   directionV,
                   directionV.Magnitude,
                   width,
                   Bone.BoneType.TYPE_METACARPAL,
                   qua);


                prev = mcpPositionV;
                next = new Vector(pipPosition[0], pipPosition[1], pipPosition[2]);

                //Rotation matrix
                x = new Vector(bases[1, 0], bases[1, 1], bases[1, 2]);
                y = new Vector(bases[1, 3], bases[1, 4], bases[1, 5]);
                z = new Vector(bases[1, 6], bases[1, 7], bases[1, 8]);

                if (isLeft)
                    qua = QuaternionHelper.generateQuaternion(-x, y, z);
                else
                    qua = QuaternionHelper.generateQuaternion(x, y, z);

                proximal = new Bone(
                   prev,
                   next,
                   (prev + next) / 2.0f,
                   directionV,
                   (next - prev).Magnitude,
                   width,
                   Bone.BoneType.TYPE_PROXIMAL,
                   qua);

                prev = new Vector(pipPosition[0], pipPosition[1], pipPosition[2]);
                next = dipPositionV;

                //Rotation matrix
                x = new Vector(bases[2, 0], bases[2, 1], bases[2, 2]);
                y = new Vector(bases[2, 3], bases[2, 4], bases[2, 5]);
                z = new Vector(bases[2, 6], bases[2, 7], bases[2, 8]);

                if (isLeft)
                    qua = QuaternionHelper.generateQuaternion(-x, y, z);
                else
                    qua = QuaternionHelper.generateQuaternion(x, y, z);

                intermediate = new Bone(
                  prev,
                   next,
                   (prev + next) / 2.0f,
                   directionV,
                   (next - prev).Magnitude,
                   width,
                   Bone.BoneType.TYPE_INTERMEDIATE,
                   qua);

                prev = dipPositionV;

                if (btipPosition.Length > 0)
                {
                    next = new Vector(btipPosition[0], btipPosition[1], btipPosition[2]);
                }
                else
                {
                    next = new Vector();
                }

                //Rotation matrix
                x = new Vector(bases[3, 0], bases[3, 1], bases[3, 2]);
                y = new Vector(bases[3, 3], bases[3, 4], bases[3, 5]);
                z = new Vector(bases[3, 6], bases[3, 7], bases[3, 8]);

                if (isLeft)
                    qua = CustomLeap.QuaternionHelper.generateQuaternion(-x, y, z);
                else
                    qua = QuaternionHelper.generateQuaternion(x, y, z);

                distal = new Bone(
                prev,
                 next,
                 (prev + next) / 2.0f,
                 directionV,
                 (next - prev).Magnitude,
                 width,
                 Bone.BoneType.TYPE_DISTAL,
                qua);

                finger = new Leap.Finger(
                       frame.Id,
                       handId,
                       id,
                       timeVisible,
                       tipPositionV,
                       tipVelocityV,
                       new Vector(direction[0], direction[1], direction[2]),
                       new Vector(stabilizedTipPosition[0], stabilizedTipPosition[1], stabilizedTipPosition[2]),
                       width,
                       length,
                       extended,
                       (Leap.Finger.FingerType)type,
                       metacarpal, proximal, intermediate, distal);


                //Adding fingers to hand
                frame.Hands[indiceMano].Fingers.Add(finger);


                #endregion

            }
            //Altro
        }
        return frame;
    }
}


    */