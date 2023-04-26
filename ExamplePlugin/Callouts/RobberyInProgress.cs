using System;
using Rage;
using Rage.Native;
using LSPD_First_Response.Mod.Callouts;
using LSPD_First_Response.Mod.API;
using System.Windows;
using System.Windows.Forms;
using LSPD_First_Response.Engine.Scripting.Entities;
using LSPD_First_Response.Engine;
using System.Net.Mail;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using RegularCallouts.Stuff;

namespace RegularCallouts.Callouts
{
    [CalloutInfo("Robbery in Progress", CalloutProbability.High)]
    public class RobberyInProgress : Callout
    {
        private Vector3 SpawnPoint;
        private Vector3 SpawnPoint1;
        private Vector3 EscapeVehicle;
        private Blip SpawnBlip;
        private bool Debrief;
        private Vector3 SpawnPoint2;
        private Vector3 SpawnPoint3;
        private Vector3 SpawnPoint4;
        private Vector3 SpawnPoint5;
        private Vector3 SpawnPoint6;
        private Vector3 SpawnPoint7;
        private Vector3 SpawnPoint8;
        private Vector3 SpawnPoint9;
        private Vector3 SpawnPoint10;
        private Vector3 SpawnPoint11;
        private Vector3 SpawnPoint12;
        private Vector3 SpawnPoint13;
        private Vector3 SpawnPoint14;
        private Vector3 SpawnPoint15;
        private Vector3 SpawnPoint16;
        private Vector3 SpawnPoint17;
        private Vector3 SpawnPoint18;
        private Vector3 SpawnPoint19;
        private Vehicle EscapeCar;
        private float EscapeVehicleFloat;
        private float RobberWatchFloat;
        private Vector3 RobberSafeCrack;
        private Ped ShopOwner;
        private bool TalkedWithShopOwner;
        private bool CarTask;
        private Rage.Object Pencil;
        private Rage.Object Paperwork;
        private Rage.Object Ziptie;
        private int CountDownTimer;
        private Vector3 RobberWatchDoor;
        private Blip SuspectBlip;
        private Vector3 ShopownerSpawn;
        private string CalloutIs;
        private bool ChaseEscapee;
        private Blip RobberBlip;
        private float ShopOwnerFloat;
        private Vector3 RobberSpawnPointAimingAtOwner;
        private bool TimerStart;
        private List<Entity> AllCalloutEntities = new List<Entity>();
        private List<Ped> Robbers = new List<Ped>();
        private bool OpenedDoor;
        private LHandle Pursuit;
        private bool PursuitActive;
        private bool EnteredCar;
        private Ped AssistOfficer;
        private bool WatchedFootage;
        private static string[] LSPDModels = new string[] { "s_m_y_cop_01", "S_F_Y_COP_01" };
        private static string[] CarModels = new string[] { "POLICE", "POLICE2", "POLICE3" };
        private bool UntiedShopowner;
        private string[] Titles = { "ASEA", "PRAIRIE", "CHINO", "TAMPA", "HABANERO", "NEON", "BALLER", "ALPHA", "SURGE" };
        private bool CalloutRunning;
        private Vector3 CameraLocation;
        private bool AttackingRobbers;
        private bool ShopOwnerBackRoom;
        private bool RobberEscaping;

        private List<string> DialogWithShopOwnerKilledSuspect = new List<string>
        {
            "~p~Cashier:~s~ Sorry to inform you Officer, but I already took care of the problem. (1/7)",
            "~b~You:~s~ Yeah I can see that. Can you tell me what happened? (2/7)",
            "~p~Cashier:~s~ This little punk came in with a knife and told me to give him all the money. (3/7)",
            "~p~Cashier:~s~ When he came around the corner I grabbed the shotgun and killed him. (4/7)",
            "~b~You:~s~ I see. I still need to investigate. Do you have a recording of it by any chance? (5/7)",
            "~p~Cashier:~s~ Of course Sir. Just let me know when you want to see it (6/7)",
            "~b~You:~s~ Will do. Thanks. And please put the gun away now. (7/7)"
        };
        private int DialogWithShopOwnerKilledSuspectIndex;

        private List<string> DialogWithShopOwnerReport = new List<string>
        {
            "~b~You:~s~ Goo day Sir. Could you tell me what happened? (1/7)",
            "~p~Cashier:~s~ I just came in from a shift switch. I checked the list of what has to be done for the day and took out the garbage. (2/7)",
            "~p~Cashier:~s~ When I came back I saw that guy behind the counter and I thought he just wanted cigarettes or something so I called him out. (3/7)",
            "~p~Cashier:~s~ He turned around and aimed a gun at me. However I saw the orange tip which toy guns have. (4/7)",
            "~p~Cashier:~s~ I jumped the counter and beat him until your partner arrived and arrested him. That's all Sir. (5/7)",
            "~b~You:~s~ Alright thanks for your information. If you wanna press additional charges for any damages he caused I will mail you the case number so you can submit that to your lawyer. (6/7)",
            "~b~Cashier:~s~ Thank you and have a nice day Officer! (7/7)"
        };
        private int DialogWithShopOwnerReportIndex;

        private List<string> DialogWithShopOwnerCuffed = new List<string>
        {
            "~p~Shopowner:~s~ Thank god you came so quickly. Did you arrest the guy already?. (2/4)",
            "~b~You:~s~ Which guy? I didn't see anyone (3/4)"
        };
        private int DialogWithShopOwnerCuffedIndex;

        private List<string> DialogWithAssistOfficer = new List<string>
        {
            "~g~Officer:~s~ Good to see you here! This guy tried to steal money with a BB gun. The cashier tackled him to the ground. Could you get a report and do the paperwork with the cashier while I book him? (1/3)",
            "~b~You:~s~ Sure no problem.I'll come see you when I'm done. (2/3)",
            "~g~Officer:~s~ Much appreciated. (3/3)"
        };
        private int DialogWithAssistOfficerIndex;

        private bool RobberEscaping2;
        private static string[] CalloutPossibility = new string[] { "RobberWithCarFlee", "ShopOwnerShotRobber", "ShopOwnerCuffed", "OfficerAssistance" };
        public override bool OnBeforeCalloutDisplayed()
        {

            SpawnPoint1 = new Vector3(-53.573719f, -1757.40393f, 29.4390278f);
            SpawnPoint2 = new Vector3(29.0805149f, -1350.8175f, 29.3384476f);
            SpawnPoint3 = new Vector3(1142.12183f, -980.789307f, 46.206604f);
            SpawnPoint4 = new Vector3(1159.98572f, -327.352814f, 69.2169724f);
            SpawnPoint5 = new Vector3(376.730438f, 322.304749f, 103.432228f);
            SpawnPoint6 = new Vector3(-1491.67432f, -384.092377f, 40.0737076f);
            SpawnPoint7 = new Vector3(-1227.03516f, -902.059631f, 12.2766676f);
            SpawnPoint8 = new Vector3(-712.166626f, -917.837952f, 19.217741f);
            SpawnPoint9 = new Vector3(-1821.59399f, 787.680603f, 138.182602f);
            SpawnPoint10 = new Vector3(-2974.45142f, 390.631042f, 15.0360699f);
            SpawnPoint11 = new Vector3(-3037.63477f, 589.943054f, 7.81816244f);
            SpawnPoint12 = new Vector3(-3238.72998f, 1004.38428f, 12.4593849f);
            SpawnPoint13 = new Vector3(1730.82507f, 6410.46826f, 34.9865913f);
            SpawnPoint14 = new Vector3(1698.18286f, 4929.70801f, 42.0891342f);
            SpawnPoint15 = new Vector3(1394.92261f, 3598.89795f, 35.0053558f);
            SpawnPoint16 = new Vector3(1965.67969f, 3739.97046f, 32.3269043f);
            SpawnPoint17 = new Vector3(2683.06738f, 3282.05762f, 55.252079f);
            SpawnPoint18 = new Vector3(543.989502f, 2673.38745f, 42.1560898f);
            SpawnPoint19 = new Vector3(2560.12402f, 385.704651f, 108.617729f);
            Vector3[] spawnpoints = new Vector3[]
            {
                SpawnPoint1,
                SpawnPoint2,
                SpawnPoint3,
                SpawnPoint4,
                SpawnPoint5,
                SpawnPoint6,
                SpawnPoint7,
                SpawnPoint8,
                SpawnPoint9,
                SpawnPoint10,
                SpawnPoint11,
                SpawnPoint12,
                SpawnPoint13,
                SpawnPoint14,
                SpawnPoint15,
                SpawnPoint16,
                SpawnPoint17,
                SpawnPoint18,
                SpawnPoint19
            };
            Vector3 closestspawnpoint = spawnpoints.OrderBy(x => x.DistanceTo(Game.LocalPlayer.Character)).First();
            if (closestspawnpoint == SpawnPoint1)
            {
                SpawnPoint = SpawnPoint1;
                RobberSpawnPointAimingAtOwner = new Vector3(-44.5858231f, -1755.59546f, 29.4213238f);
                RobberSafeCrack = new Vector3(-42.9749069f, -1748.65051f, 29.4202423f);
                RobberWatchDoor = new Vector3(-53.3119164f, -1753.81165f, 29.4209957f);
                RobberWatchFloat = 176.785614f;
                EscapeVehicle = new Vector3(-59.5966072f, -1755.31812f, 28.9958134f);
                EscapeVehicleFloat = 51.9449005f;
                ShopownerSpawn = new Vector3(-46.8848534f, -1759.17395f, 29.4210148f);
                ShopOwnerFloat = -3.50276709f;
                CameraLocation = new Vector3(-57.2281456f, -1752.14111f, 31.573143f);
                Game.LogTrivial("SpawnPoint 1 Chosen");
            }
            if (closestspawnpoint == SpawnPoint2)
            {
                SpawnPoint = SpawnPoint2;
                RobberSpawnPointAimingAtOwner = new Vector3(24.6676483f, -1343.67078f, 29.5005493f);
                RobberSafeCrack = new Vector3(28.1021824f, -1340.0625f, 29.5008564f);
                RobberWatchDoor = new Vector3(27.0302448f, -1348.047f, 29.5009365f);
                RobberWatchFloat = -126.68045f;
                EscapeVehicle = new Vector3(26.8030281f, -1356.62366f, 29.1031227f);
                EscapeVehicleFloat = 90.1167908f;
                ShopownerSpawn = new Vector3(24.3448887f, -1346.38062f, 29.5008087f);
                ShopOwnerFloat = -88.2226563f;
                CameraLocation = new Vector3(34.3101959f, -1348.65027f, 31.6696491f);
                Game.LogTrivial("SpawnPoint 2 Chosen");
            }
            if (closestspawnpoint == SpawnPoint3)
            {
                SpawnPoint = SpawnPoint3;
                RobberSpawnPointAimingAtOwner = new Vector3(1133.76038f, -979.67865f, 46.4192543f);
                RobberSafeCrack = new Vector3(1130.66992f, -981.906616f, 46.4195671f);
                RobberWatchDoor = new Vector3(1139.26196f, -981.973267f, 46.4189606f);
                RobberWatchFloat = -63.6493759f;
                EscapeVehicle = new Vector3(1147.68909f, -981.215759f, 45.9059029f);
                EscapeVehicleFloat = -177.963211f;
                ShopownerSpawn = new Vector3(1133.7876f, -982.789185f, 46.4189682f);
                ShopOwnerFloat = -77.7892227f;
                CameraLocation = new Vector3(1140.80725f, -980.796875f, 48.3169823f);
                Game.LogTrivial("SpawnPoint 3 Chosen");
            }
            if (closestspawnpoint == SpawnPoint4)
            {
                SpawnPoint = SpawnPoint4;
                RobberSpawnPointAimingAtOwner = new Vector3(1164.11816f, -319.613464f, 69.2019043f);
                RobberSafeCrack = new Vector3(1160.41809f, -314.320282f, 69.2014847f);
                RobberWatchDoor = new Vector3(1157.56372f, -325.437225f, 69.2013321f);
                RobberWatchFloat = -118.101425f;
                EscapeVehicle = new Vector3(1167.08545f, -328.913666f, 68.8469315f);
                EscapeVehicleFloat = -80.7740097f;
                ShopownerSpawn = new Vector3(1165.02258f, -322.593903f, 69.2016449f);
                ShopOwnerFloat = 100.833664f;
                CameraLocation = new Vector3(1153.50806f, -327.002991f, 71.464798f);
                Game.LogTrivial("SpawnPoint 5 Chosen");
            }
            if (closestspawnpoint == SpawnPoint5)
            {
                SpawnPoint = SpawnPoint5;
                RobberSpawnPointAimingAtOwner = new Vector3(373.582184f, 329.994843f, 103.562134f);
                RobberSafeCrack = new Vector3(378.024261f, 332.549286f, 103.562996f);
                RobberWatchDoor = new Vector3(375.132111f, 325.007843f, 103.563095f);
                RobberWatchFloat = -152.020966f;
                EscapeVehicle = new Vector3(370.745667f, 318.8638f, 103.25956f);
                EscapeVehicleFloat = 75.4956436f;
                ShopownerSpawn = new Vector3(372.477325f, 326.819031f, 103.562561f);
                ShopOwnerFloat = -109.320007f;
                CameraLocation = new Vector3(381.77652f, 322.719452f, 105.819504f);
                Game.LogTrivial("SpawnPoint 5 Chosen");
            }
            if (closestspawnpoint == SpawnPoint6)
            {
                SpawnPoint = SpawnPoint6;
                RobberSpawnPointAimingAtOwner = new Vector3(-1484.2179f, -380.096619f, 40.1665993f);
                RobberSafeCrack = new Vector3(-1482.29602f, -376.944092f, 40.1668701f);
                RobberWatchDoor = new Vector3(-1490.3739f, -381.14444f, 40.1665611f);
                RobberWatchFloat = 173.406876f;
                EscapeVehicle = new Vector3(-1497.08826f, -382.958405f, 40.1559868f);
                EscapeVehicleFloat = 46.7011986f;
                ShopownerSpawn = new Vector3(-1485.95056f, -377.77478f, 40.1667671f);
                ShopOwnerFloat = 134.086258f;
                CameraLocation = new Vector3(-1490.49487f, -383.345245f, 42.0656433f);
                Game.LogTrivial("SpawnPoint 6 Chosen");
            }
            if (closestspawnpoint == SpawnPoint7)
            {
                SpawnPoint = SpawnPoint7;
                RobberSpawnPointAimingAtOwner = new Vector3(-1224.44641f, -910.174805f, 12.3222408f);
                RobberSafeCrack = new Vector3(-1221.47705f, -912.142578f, 12.3228035f);
                RobberWatchDoor = new Vector3(-1224.23938f, -903.535645f, 12.3226423f);
                RobberWatchFloat = 173.406876f;
                EscapeVehicle = new Vector3(-1227.9696f, -896.978821f, 12.1339693f);
                EscapeVehicleFloat = -55.8543243f;
                ShopownerSpawn = new Vector3(-1221.96802f, -908.623962f, 12.3225746f);
                ShopOwnerFloat = 32.3395386f;
                CameraLocation = new Vector3(-1226.3479f, -903.108765f, 14.2901497f);
                Game.LogTrivial("SpawnPoint 7 Chosen");
            }
            if (closestspawnpoint == SpawnPoint8)
            {
                SpawnPoint = SpawnPoint8;
                RobberSpawnPointAimingAtOwner = new Vector3(-706.145874f, -910.718506f, 19.2194347f);
                RobberSafeCrack = new Vector3(-708.829346f, -904.57312f, 19.2190781f);
                RobberWatchDoor = new Vector3(-713.503418f, -914.679565f, 19.2197552f);
                RobberWatchFloat = -133.345032f;
                EscapeVehicle = new Vector3(-715.35498f, -921.199707f, 18.8289986f);
                EscapeVehicleFloat = -55.8543243f;
                ShopownerSpawn = new Vector3(-705.624756f, -913.845459f, 19.2188339f);
                ShopOwnerFloat = 88.3340225f;
                CameraLocation = new Vector3(-717.87915f, -915.881958f, 21.4965343f);
                Game.LogTrivial("SpawnPoint 8 Chosen");
            }
            if (closestspawnpoint == SpawnPoint9)
            {
                SpawnPoint = SpawnPoint9;
                RobberSpawnPointAimingAtOwner = new Vector3(-1822.10791f, 796.44043f, 138.101486f);
                RobberSafeCrack = new Vector3(-1828.04236f, 799.270569f, 138.170227f);
                RobberWatchDoor = new Vector3(-1824.8717f, 788.915771f, 138.22554f);
                RobberWatchFloat = -97.9066162f;
                EscapeVehicle = new Vector3(-1821.61401f, 782.611755f, 137.705795f);
                EscapeVehicleFloat = -138.003754f;
                ShopownerSpawn = new Vector3(-1819.74609f, 794.514404f, 138.082825f);
                ShopOwnerFloat = 132.345734f;
                CameraLocation = new Vector3(-1827.23047f, 784.624329f, 140.563416f);
                Game.LogTrivial("SpawnPoint 9 Chosen");
            }
            if (closestspawnpoint == SpawnPoint10)
            {
                SpawnPoint = SpawnPoint10;
                RobberSpawnPointAimingAtOwner = new Vector3(-2966.597411f, 387.970154f, 15.0397987f);
                RobberSafeCrack = new Vector3(-2963.06372f, 388.744049f, 15.0399561f);
                RobberWatchDoor = new Vector3(-2971.38159f, 391.607239f, 15.0394726f);
                RobberWatchFloat = 128.053192f;
                EscapeVehicle = new Vector3(-2981.15991f, 395.48819f, 14.7027473f);
                EscapeVehicleFloat = -7.75609589f;
                ShopownerSpawn = new Vector3(-2966.26392f, 390.90329f, 15.0391102f);
                ShopOwnerFloat = 86.999588f;
                CameraLocation = new Vector3(-2973.22217f, 390.429718f, 16.9617844f);
                Game.LogTrivial("SpawnPoint 10 Chosen");
            }
            if (closestspawnpoint == SpawnPoint11)
            {
                SpawnPoint = SpawnPoint11;
                RobberSpawnPointAimingAtOwner = new Vector3(-3042.44043f, 583.612305f, 7.90557241f);
                RobberSafeCrack = new Vector3(-3047.04663f, 585.655762f, 7.90520191f);
                RobberWatchDoor = new Vector3(-3039.76733f, 587.488403f, 7.905375f);
                RobberWatchFloat = -34.6746178f;
                EscapeVehicle = new Vector3(-3030.4834f, 587.31189f, 7.50080061f);
                EscapeVehicleFloat = -162.374893f;
                ShopownerSpawn = new Vector3(-3039.3147f, 584.242004f, 7.90541744f);
                ShopOwnerFloat = 15.1286039f;
                CameraLocation = new Vector3(-3040.67871f, 594.307007f, 10.1616259f);
                Game.LogTrivial("SpawnPoint 11 Chosen");
            }
            if (closestspawnpoint == SpawnPoint12)
            {
                SpawnPoint = SpawnPoint12;
                RobberSpawnPointAimingAtOwner = new Vector3(-3246.052f, 1000.2746f, 12.8274107f);
                RobberSafeCrack = new Vector3(-3249.3252f, 1004.01337f, 12.8268814f);
                RobberWatchDoor = new Vector3(-3241.81519f, 1002.74103f, 12.8265533f);
                RobberWatchFloat = -53.8019447f;
                EscapeVehicle = new Vector3(-3236.58887f, 1008.55951f, 12.0815496f);
                EscapeVehicleFloat = -2.13416147f;
                ShopownerSpawn = new Vector3(-3242.32642f, 999.805237f, 12.8268347f);
                ShopOwnerFloat = -5.47143221f;
                CameraLocation = new Vector3(-3240.06934f, 1009.65308f, 15.0815687f);
                Game.LogTrivial("SpawnPoint 12 Chosen");
            }
            if (closestspawnpoint == SpawnPoint13)
            {
                SpawnPoint = SpawnPoint13;
                RobberSpawnPointAimingAtOwner = new Vector3(1729.92908f, 6418.45508f, 35.0483856f);
                RobberSafeCrack = new Vector3(1734.42859f, 6420.30518f, 35.0404282f);
                RobberWatchDoor = new Vector3(1730.23083f, 6414.37061f, 35.0413895f);
                RobberWatchFloat = -165.332718f;
                EscapeVehicle = new Vector3(1719.52148f, 6409.22412f, 33.7224541f);
                EscapeVehicleFloat = 151.925095f;
                ShopownerSpawn = new Vector3(1727.80518f, 6415.59277f, 35.0409622f);
                ShopOwnerFloat = -117.019249f;
                CameraLocation = new Vector3(1736.06641f, 6409.64795f, 37.2925262f);
                Game.LogTrivial("SpawnPoint 13 Chosen");
            }
            if (closestspawnpoint == SpawnPoint14)
            {
                SpawnPoint = SpawnPoint14;
                RobberSpawnPointAimingAtOwner = new Vector3(1700.29114f, 4921.01074f, 42.0762558f);
                RobberSafeCrack = new Vector3(1707.33533f, 4920.08594f, 42.0679283f);
                RobberWatchDoor = new Vector3(1701.48987f, 4930.2373f, 42.0679588f);
                RobberWatchFloat = 109.394035f;
                EscapeVehicle = new Vector3(1697.84241f, 4935.06494f, 41.8952446f);
                EscapeVehicleFloat = -17.0690365f;
                ShopownerSpawn = new Vector3(1697.1217f, 4923.22852f, 42.0763435f);
                ShopOwnerFloat = -40.0156288f;
                CameraLocation = new Vector3(1702.9679f, 4933.8418f, 44.332653f);
                Game.LogTrivial("SpawnPoint 14 Chosen");
            }
            if (closestspawnpoint == SpawnPoint15)
            {
                SpawnPoint = SpawnPoint15;
                RobberSpawnPointAimingAtOwner = new Vector3(1388.98181f, 3604.50879f, 34.9674416f);
                RobberSafeCrack = new Vector3(1393.94983f, 3608.73975f, 34.9689445f);
                RobberWatchDoor = new Vector3(1395.45593f, 3602.20483f, 34.9686317f);
                RobberWatchFloat = 143.016251f;
                EscapeVehicle = new Vector3(1393.75269f, 3593.58813f, 34.7193146f);
                EscapeVehicleFloat = 136.994644f;
                ShopownerSpawn = new Vector3(1392.42944f, 3606.42188f, 34.9678268f);
                ShopOwnerFloat = -162.589005f;
                CameraLocation = new Vector3(1390.55383f, 3598.94995f, 37.2112274f);
                Game.LogTrivial("SpawnPoint 15 Chosen");
            }
            if (closestspawnpoint == SpawnPoint16)
            {
                SpawnPoint = SpawnPoint16;
                RobberSpawnPointAimingAtOwner = new Vector3(1958.37585f, 3743.48218f, 32.3440437f);
                RobberSafeCrack = new Vector3(1959.43225f, 3748.23853f, 32.3440666f);
                RobberWatchDoor = new Vector3(1965.98877f, 3743.10498f, 32.356945f);
                RobberWatchFloat = 157.75737f;
                EscapeVehicle = new Vector3(1964.4021f, 3733.68188f, 32.0522995f);
                EscapeVehicleFloat = 124.033951f;
                ShopownerSpawn = new Vector3(1959.73962f, 3740.29517f, 32.3475723f);
                ShopOwnerFloat = -63.4880066f;
                CameraLocation = new Vector3(1969.27917f, 3743.75244f, 34.5927925f);
                Game.LogTrivial("SpawnPoint 16 Chosen");
            }
            if (closestspawnpoint == SpawnPoint17)
            {
                SpawnPoint = SpawnPoint17;
                RobberSpawnPointAimingAtOwner = new Vector3(2675.00537f, 3281.66162f, 55.2448235f);
                RobberSafeCrack = new Vector3(2673.44995f, 3286.5293f, 55.2453156f);
                RobberWatchDoor = new Vector3(2679.64966f, 3282.3186f, 55.2449646f);
                RobberWatchFloat = -83.5552979f;
                EscapeVehicle = new Vector3(2683.98755f, 3276.45605f, 55.0564537f);
                EscapeVehicleFloat = 174.554077f;
                ShopownerSpawn = new Vector3(2677.51514f, 3279.53442f, 55.2446175f);
                ShopOwnerFloat = -34.0130615f;
                CameraLocation = new Vector3(2683.99219f, 3287.37207f, 57.5054703f);
                Game.LogTrivial("SpawnPoint 17 Chosen");
            }
            if (closestspawnpoint == SpawnPoint18)
            {
                SpawnPoint = SpawnPoint18;
                RobberSpawnPointAimingAtOwner = new Vector3(549.429993f, 2667.49341f, 42.1596985f);
                RobberSafeCrack = new Vector3(546.514465f, 2663.47778f, 42.1596603f);
                RobberWatchDoor = new Vector3(544.227051f, 2669.99951f, 42.159668f);
                RobberWatchFloat = -0.764351726f;
                EscapeVehicle = new Vector3(543.177795f, 2678.604f, 42.0356941f);
                EscapeVehicleFloat = 79.2306671f;
                ShopownerSpawn = new Vector3(549.174377f, 2671.10815f, 42.1604767f);
                ShopOwnerFloat = 98.3221283f;
                CameraLocation = new Vector3(539.160889f, 2671.4187f, 44.4138107f);
                Game.LogTrivial("SpawnPoint 18 Chosen");
            }
            if (closestspawnpoint == SpawnPoint19)
            {
                SpawnPoint = SpawnPoint19;
                RobberSpawnPointAimingAtOwner = new Vector3(2553.57397f, 381.139618f, 108.618851f);
                RobberSafeCrack = new Vector3(2549.64038f, 384.950989f, 108.619621f);
                RobberWatchDoor = new Vector3(2556.32251f, 383.472412f, 108.619507f);
                RobberWatchFloat = -66.9885483f;
                EscapeVehicle = new Vector3(2566.35693f, 379.602051f, 108.28009f);
                EscapeVehicleFloat = -167.491959f;
                ShopownerSpawn = new Vector3(2556.71362f, 380.608521f, 108.622589f);
                ShopOwnerFloat = -6.18624973f;
                CameraLocation = new Vector3(2558.95557f, 390.578125f, 110.887939f);
                Game.LogTrivial("SpawnPoint 19 Chosen");
            }


            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 60f);
            CalloutMessage = "Robbery in Progress";
            CalloutPosition = SpawnPoint;
            Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_DISTURBING_THE_PEACE IN_OR_ON_POSITION UNITS_RESPOND_CODE_03", SpawnPoint);

            return base.OnBeforeCalloutDisplayed();
        }
        public override bool OnCalloutAccepted()
        {
            CalloutIs = CalloutPossibility[new Random().Next(CalloutPossibility.Length)];
            Game.DisplayHelp("You can press~r~ " + Stuff.Settings.EndCalloutKey.ToString() + "~s~ anytime to end the callout.");
            SpawnBlip = new Blip(SpawnPoint, 40f);
            SpawnBlip.Color = Color.Yellow;
            SpawnBlip.Alpha = 0.5f;
            Game.LocalPlayer.Character.RelationshipGroup = "PLAYER";
            SpawnBlip.EnableRoute(Color.Yellow);
            CountDownTimer = 0;
            CalloutLos();
            return base.OnCalloutAccepted();
        }
        private void CalloutLos()
        {
            CalloutRunning = true;
            base.Process();
            try
            {
                GameFiber.StartNew(delegate
                {
                    while (Vector3.Distance(Game.LocalPlayer.Character.Position, SpawnPoint) > 350f)
                    {
                        GameFiber.Yield();
                        GameEnd();
                    }
                    if (Game.LocalPlayer.Character.IsInAnyVehicle(false))
                    {
                        AllCalloutEntities.Add(Game.LocalPlayer.Character.CurrentVehicle);
                        Ped[] passengers = Game.LocalPlayer.Character.CurrentVehicle.Passengers;
                        if (passengers.Length > 0)
                        {
                            foreach (Ped passenger in passengers)
                            {
                                AllCalloutEntities.Add(passenger);
                            }
                        }
                    }
                    GameFiber.Yield();
                    CreateSpeedZone();
                    GameFiber.Yield();
                    ClearUnrelatedEntities();
                    Game.LogTrivial("Unrelated entities cleared");
                    GameFiber.Yield();
                    MakeNearbyPedsFlee();
                    GameFiber.Yield();
                    Game.LogTrivial("Initialisation complete, Spawning Callout");
                    if(CalloutIs == "RobberWithCarFlee")
                    {
                        RobberSpawnWithEscape();
                        Game.LogTrivial("RobberWithCarFlee Variation");
                        GameFiber.Yield();
                    }
                    else if(CalloutIs == "ShopOwnerShotRobber")
                    {
                        ShowOwnerKilledRobberSpawn();
                        Game.LogTrivial("ShopOwnerShotRobber Variation");
                        GameFiber.Yield();
                    }
                    else if (CalloutIs == "ShopOwnerCuffed")
                    {
                        ShopOwnerCuffedSpawning();
                        Game.LogTrivial("ShopOwnerCuffed Variation");
                        GameFiber.Yield();
                    }
                    else if (CalloutIs == "OfficerAssistance")
                    {
                        OfficerNeedsAssistanceSpawn();
                        Game.LogTrivial("OfficerAssistance Variation");
                        Game.DisplayNotification("~b~Attention to Dispatch:~s~ This is Officer "+ Functions.GetPersonaForPed(AssistOfficer).Surname +" speaking. I need one more backup unit. The Suspect is already in custody");
                        GameFiber.Yield();
                    }
                    while (CalloutRunning)
                    {
                        GameFiber.Yield();
                        GameEnd();

                        Game.LocalPlayer.Character.CanAttackFriendlies = false;
                        Game.SetRelationshipBetweenRelationshipGroups("COP", "ROB", Relationship.Hate);
                        Game.SetRelationshipBetweenRelationshipGroups("ROB", "COP", Relationship.Hate);
                        Game.SetRelationshipBetweenRelationshipGroups("ROB", "PLAYER", Relationship.Hate);
                        Game.SetRelationshipBetweenRelationshipGroups("PLAYER", "ROB", Relationship.Hate);
                        Game.SetRelationshipBetweenRelationshipGroups("COP", "PLAYER", Relationship.Respect);
                        Game.SetRelationshipBetweenRelationshipGroups("PLAYER", "COP", Relationship.Respect);

                        if (CalloutIs == "RobberWithCarFlee")
                        {
                            if (Game.LocalPlayer.Character.DistanceTo(SpawnPoint) <= 8f && TimerStart == false)
                            {
                                AttackRobbers();
                                Game.DisplaySubtitle("~r~Robber:~s~ Don't come any closer or the cashier will die!");
                                SpawnBlip.Delete();
                                TimerStart = true;
                            }
                            if (TimerStart == true && RobberEscaping == false && AttackingRobbers == false)
                            {
                                GameFiber.StartNew(delegate
                                {
                                    while(RobberEscaping == false)
                                    {
                                        GameFiber.Yield();
                                        AttackRobbers();
                                    }
                                });
                                while(true)
                                {
                                    GameFiber.Yield();
                                    AttackRobbers();
                                    GameEnd();
                                    CountDownTimer++;
                                    if(AttackingRobbers == true)
                                    {
                                        break;
                                    }
                                    GameFiber.Sleep(1000);
                                    if (CountDownTimer == 5)
                                    {
                                        Game.DisplaySubtitle("~r~Robber:~s~ We're just taking the money and we'll leave, no one has to get hurt.");
                                    }
                                    if (CountDownTimer == 10)
                                    {
                                        Game.DisplaySubtitle("~r~Robber:~s~ We're leaving. Don't try anything!");
                                        foreach (Ped i in Robbers)
                                        {
                                            if (!i.IsInAnyVehicle(false))
                                            {
                                                i.Tasks.FollowNavigationMeshToPosition(EscapeCar.Position, 0f, 2f);
                                            }
                                        }
                                        break;
                                    }
                                }
                                RobberEscaping = true;

                            }
                            if (RobberEscaping == true && RobberEscaping2 == false && AttackingRobbers == false)
                            {
                                AttackRobbers();
                                foreach (Ped i in Robbers)
                                {
                                    if (!i.IsInAnyVehicle(false) && i.DistanceTo(EscapeCar) <= 5f && !i.IsGettingIntoVehicle)
                                    {
                                        i.Tasks.EnterVehicle(EscapeCar, -2, 2f);
                                    }
                                }
                                if (Robbers.All(rob => rob.IsInAnyVehicle(false) == true))
                                {
                                    RobberEscaping2 = true;
                                }
                            }
                            if (RobberEscaping2 == true && PursuitActive == false)
                            {

                                if(!Functions.IsPedInPursuit(Game.LocalPlayer.Character))
                                {
                                    Pursuit = Functions.CreatePursuit();
                                    foreach (Ped i in Robbers)
                                    {
                                        Functions.AddPedToPursuit(Pursuit, i);
                                    }
                                    Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                                    PursuitActive = true;
                                }
                            }
                            if (PursuitActive == true)
                            {
                                if (Robbers.All(rob => rob.IsDead || rob.IsCuffed))
                                {
                                    this.End();
                                }
                            }
                            if (AttackingRobbers)
                            {
                                foreach(Ped i in Robbers)
                                {
                                    i.Tasks.FightAgainst(Game.LocalPlayer.Character, -1);
                                }
                                if (Robbers.All(rob => rob.IsDead || rob.IsCuffed))
                                {
                                    this.End();
                                }
                            }
                        }
                        if (CalloutIs == "ShopOwnerShotRobber")
                        {
                            if (Game.LocalPlayer.Character.DistanceTo(SpawnPoint) <= 10f && SpawnBlip.Exists())
                            {
                                Game.DisplayHelp("Investigate the situation.");
                                SuspectBlip = ShopOwner.AttachBlip();
                                SuspectBlip.Color = Color.Purple;
                                SpawnBlip.Delete();
                            }
                            if (Game.LocalPlayer.Character.DistanceTo(ShopOwner) <= 5f && TalkedWithShopOwner == false)
                            {
                                Utils.TurnToFaceEntity(ShopOwner, Game.LocalPlayer.Character, -1);
                            }
                            if (Game.LocalPlayer.Character.DistanceTo(ShopOwner) <= 3f && TalkedWithShopOwner == false)
                            {
                                Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to speak.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    if (DialogWithShopOwnerKilledSuspectIndex < DialogWithShopOwnerKilledSuspect.Count)
                                    {
                                        Game.DisplaySubtitle(DialogWithShopOwnerKilledSuspect[DialogWithShopOwnerKilledSuspectIndex]);
                                        DialogWithShopOwnerKilledSuspectIndex++;
                                    }
                                    if (DialogWithShopOwnerKilledSuspectIndex == DialogWithShopOwnerKilledSuspect.Count)
                                    {
                                        ShopOwner.Inventory.GiveNewWeapon("weapon_unarmed", 1, true);
                                        GameFiber.Sleep(1000);
                                        TalkedWithShopOwner = true;
                                    }
                                }
                            }
                            if (Game.LocalPlayer.Character.DistanceTo(ShopOwner) <= 3f && TalkedWithShopOwner == true && ShopOwnerBackRoom == false)
                            {
                                Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to speak.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    Game.DisplaySubtitle("~b~You:~s~ Ok let's watch the Camera Footage.");
                                    ShopOwner.Tasks.FollowNavigationMeshToPosition(RobberSafeCrack, Game.LocalPlayer.Character.Heading, 1f).WaitForCompletion();
                                    ShopOwnerBackRoom = true;
                                }
                            }
                            if (Game.LocalPlayer.Character.DistanceTo(ShopOwner) <= 2f && ShopOwnerBackRoom == true && WatchedFootage == false)
                            {
                                Utils.TurnToFaceEntity(ShopOwner, Game.LocalPlayer.Character, -1);
                                Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to watch the footage.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    WatchCameraFootage();
                                    WatchedFootage = true;
                                }
                            }
                            if (WatchedFootage == true && SuspectBlip.Exists())
                            {
                                Game.DisplaySubtitle("~b~You:~s~ Alright thanks for the footage.");
                                Game.DisplayHelp("You can press~r~ " + Stuff.Settings.EndCalloutKey.ToString() + "~s~ anytime to end the callout.");
                                SuspectBlip.Delete();
                            }
                        }
                        if (CalloutIs == "ShopOwnerCuffed")
                        {
                            if (Game.LocalPlayer.Character.DistanceTo(SpawnPoint) <= 10f && SpawnBlip.Exists() && ChaseEscapee == false)
                            {
                                Game.DisplayHelp("Investigate the situation.");
                                if(ShopOwner.IsOnScreen)
                                {
                                    SuspectBlip = ShopOwner.AttachBlip();
                                    Game.DisplaySubtitle("~p~Shopowner:~s~ *muffled screaming*");
                                    SuspectBlip.Color = Color.Purple;
                                    foreach(Ped i in Robbers)
                                    {
                                        i.Tasks.CruiseWithVehicle(15);
                                    }
                                    SpawnBlip.Delete();
                                }
                            }
                            if (Game.LocalPlayer.Character.DistanceTo(ShopOwner) <= 5f)
                            {
                                ShopOwner.TurnToFaceEntity(Game.LocalPlayer.Character, -1);
                            }
                            if (Game.LocalPlayer.Character.DistanceTo(ShopOwner) <= 2f && UntiedShopowner == false)
                            {
                                Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to speak.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    Game.DisplaySubtitle("~b~You:~s~ Hey are you alright? Let me untie you real quick (1/4)");
                                    Game.LocalPlayer.Character.Tasks.FollowNavigationMeshToPosition(ShopOwner.GetOffsetPositionFront(1f), ShopOwner.Heading + 180f, 1f).WaitForCompletion();
                                    Game.LocalPlayer.Character.Tasks.PlayAnimation("mp_arresting", "a_uncuff", 1f, AnimationFlags.SecondaryTask);
                                    GameFiber.Sleep(5000);
                                    ShopOwner.Tasks.Clear();
                                    UntiedShopowner = true;
                                }
                            }
                            if (Game.LocalPlayer.Character.DistanceTo(ShopOwner) <= 2f && UntiedShopowner == true && TalkedWithShopOwner == false)
                            {
                                Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to speak.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    if (DialogWithShopOwnerCuffedIndex < DialogWithShopOwnerCuffed.Count)
                                    {
                                        Game.DisplaySubtitle(DialogWithShopOwnerCuffed[DialogWithShopOwnerCuffedIndex]);
                                        DialogWithShopOwnerCuffedIndex++;
                                    }
                                    if (DialogWithShopOwnerCuffedIndex == DialogWithShopOwnerCuffed.Count)
                                    {
                                        ShopOwner.Inventory.GiveNewWeapon("weapon_unarmed", 1, true);
                                        GameFiber.Sleep(1000);
                                        TalkedWithShopOwner = true;
                                    }
                                }
                            }
                            if (Game.LocalPlayer.Character.DistanceTo(ShopOwner) <= 2f && TalkedWithShopOwner == true && ChaseEscapee == false)
                            {
                                Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to speak.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    Game.DisplaySubtitle("~p~Shopowner:~s~ His license plate is~r~ "+ EscapeCar.LicensePlate.ToString()+"~s~ (4/4)");
                                    Functions.PlayPlayerRadioAction(Functions.GetPlayerRadioAction(), 4000);
                                    Game.DisplayNotification("~b~Attention to Dispatch:~s~ I need a search for a vehicle with license plate " + EscapeCar.LicensePlate.ToString() + ". The owner is affiliated with the Robbery in progress");
                                    GameFiber.Sleep(4000);
                                    Game.DisplayNotification("~b~Attention to responding Unit:~s~ The vehicle was spotted in this ~r~Location~s~.");
                                    GameFiber.Sleep(1000);
                                    Game.DisplaySubtitle("~p~Shopowner:~s~ Go get that son of a bitch!");
                                    SuspectBlip.Delete();
                                    ChaseEscapee = true;
                                }
                            }
                            if (ChaseEscapee == true && ShopOwnerBackRoom == false)
                            {
                                foreach(Ped i in Robbers)
                                {
                                   SpawnBlip = i.AttachBlip();
                                }
                                SpawnBlip.Color = Color.Red;
                                SpawnBlip.EnableRoute(Color.Red);
                                ShopOwnerBackRoom = true;
                            }
                            if (ShopOwnerBackRoom == true)
                            {
                                foreach (Ped i in Robbers)
                                {
                                    if (i.IsDead || i.IsCuffed)
                                    {
                                        this.End();
                                    }
                                }
                            }
                        }
                        if (CalloutIs == "OfficerAssistance")
                        {
                            if (Game.LocalPlayer.Character.DistanceTo(SpawnPoint) <= 10f && SpawnBlip.Exists())
                            {
                                Game.DisplayHelp("Make contact with the fellow ~g~Officer~s~.");
                                SuspectBlip = AssistOfficer.AttachBlip();
                                SuspectBlip.Color = Color.Green;
                                SpawnBlip.Delete();
                            }
                            if (Game.LocalPlayer.Character.DistanceTo(AssistOfficer) <= 5f && UntiedShopowner == false)
                            {
                                AssistOfficer.TurnToFaceEntity(Game.LocalPlayer.Character, -1);
                            }
                            if (Game.LocalPlayer.Character.DistanceTo(AssistOfficer) <= 2f && UntiedShopowner == false)
                            {
                                Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to speak.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    if (DialogWithAssistOfficerIndex < DialogWithAssistOfficer.Count)
                                    {
                                        Game.DisplaySubtitle(DialogWithAssistOfficer[DialogWithAssistOfficerIndex]);
                                        DialogWithAssistOfficerIndex++;
                                    }
                                    if (DialogWithAssistOfficerIndex == DialogWithAssistOfficer.Count)
                                    {
                                        SuspectBlip.Delete();
                                        SuspectBlip = ShopOwner.AttachBlip();
                                        SuspectBlip.Color = Color.Purple;
                                        UntiedShopowner = true;
                                    }
                                }
                            }
                            if (UntiedShopowner == true && Debrief == false)
                            {
                                if (CarTask == false)
                                {
                                    AssistOfficer.Tasks.FollowNavigationMeshToPosition(EscapeCar.GetOffsetPositionFront(2f), EscapeCar.Heading, 1f);
                                    foreach (Ped i in Robbers)
                                    {
                                        i.Tasks.FollowNavigationMeshToPosition(EscapeCar.GetOffsetPositionFront(2f), EscapeCar.Heading, 1f);
                                    }
                                    CarTask = true;
                                }
                                if (AssistOfficer.DistanceTo(EscapeCar) <= 4f && Robbers.Any(x => x.DistanceTo(EscapeCar) <= 4f) && OpenedDoor == false)
                                {
                                    AssistOfficer.Tasks.EnterVehicle(EscapeCar, 2, EnterVehicleFlags.DoNotEnter);
                                    AssistOfficer.Tasks.FollowNavigationMeshToPosition(EscapeCar.GetOffsetPositionRight(3f), EscapeCar.Heading, 1f);
                                    OpenedDoor = true;
                                }
                                if (OpenedDoor == true && EnteredCar == false)
                                {
                                    foreach(Ped i in Robbers)
                                    {
                                        i.Tasks.EnterVehicle(EscapeCar, 2);
                                    }
                                    EscapeCar.IsSirenOn = false;
                                    AssistOfficer.Tasks.EnterVehicle(EscapeCar, -1);
                                    if (AssistOfficer.IsInAnyVehicle(false) == true && Robbers.Any(x => x.IsInAnyVehicle(false) == true))
                                    {
                                        EnteredCar = true;
                                    }
                                }
                                if (Game.LocalPlayer.Character.DistanceTo(ShopOwner) <= 5f)
                                {
                                    ShopOwner.TurnToFaceEntity(Game.LocalPlayer.Character, -1);
                                }
                                if (Game.LocalPlayer.Character.DistanceTo(ShopOwner) <= 2f && ShopOwnerBackRoom == false)
                                {
                                    Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to speak.");
                                    if (Game.IsKeyDown(Settings.DialogKey))
                                    {
                                        if(ChaseEscapee == false)
                                        {
                                            ToggleReport(Game.LocalPlayer.Character, true);
                                            ChaseEscapee = true;
                                        }
                                        if (DialogWithShopOwnerReportIndex < DialogWithShopOwnerReport.Count)
                                        {
                                            Game.DisplaySubtitle(DialogWithShopOwnerReport[DialogWithShopOwnerReportIndex]);
                                            DialogWithShopOwnerReportIndex++;
                                        }
                                        if (DialogWithShopOwnerReportIndex == DialogWithShopOwnerReport.Count)
                                        {
                                            ToggleReport(Game.LocalPlayer.Character, false);
                                            ShopOwnerBackRoom = true;
                                        }
                                    }
                                }
                                if (ShopOwnerBackRoom == true && EnteredCar == true)
                                {
                                    Game.DisplayHelp("Go back to your ~g~Officer~s~ to debrief.");
                                    SuspectBlip = AssistOfficer.AttachBlip();
                                    SuspectBlip.Color = Color.Green;
                                    Debrief = true;
                                }
                            }
                            if (Debrief == true)
                            {
                                if (Game.LocalPlayer.Character.DistanceTo(AssistOfficer) <= 2f)
                                {
                                    Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to speak.");
                                    if (Game.IsKeyDown(Settings.DialogKey))
                                    {
                                        Game.DisplaySubtitle("~g~Officer:~s~ Thanks for the help! I'll see you on the station. Bye!");
                                        this.End();
                                    }
                                }
                            }
                        }
                    }
                });
            }
            catch(Exception e)
            {
                Game.LogTrivial("Crash averted" + e);
            }
        }
        public override void End()
        {
            CalloutRunning = false;
            if (SpawnBlip.Exists()) { SpawnBlip.Delete(); }
            if (SuspectBlip.Exists()) { SuspectBlip.Delete(); }
            if (EscapeCar.Exists()) { EscapeCar.Dismiss(); }
            if (ShopOwner.Exists()) { ShopOwner.Dismiss(); }
            if (Pencil.Exists()) { Pencil.Delete(); }
            if (Paperwork.Exists()) { Paperwork.Delete(); }
            if (AssistOfficer.Exists()) { AssistOfficer.Dismiss(); }
            foreach (Ped i in Robbers)
            {
                if (i.GetAttachedBlips().Count() != 0)
                {
                    if (i.GetAttachedBlips().OrderBy(x => x.DistanceTo(i)).First().Exists())
                    {
                        i.GetAttachedBlips().OrderBy(x => x.DistanceTo(i)).First().Delete();
                    }
                }
                i.Dismiss();
            }
            Functions.PlayScannerAudio("WE_ARE_CODE_4 NO_FURTHER_UNITS_REQUIRED");
            base.End();
        }

        private void CreateSpeedZone()
        {
            GameFiber.StartNew(delegate
            {
                while (CalloutRunning)
                {
                    GameFiber.Yield();

                    foreach (Vehicle veh in World.GetEntities(SpawnPoint, 75f, GetEntitiesFlags.ConsiderGroundVehicles | GetEntitiesFlags.ExcludePoliceCars | GetEntitiesFlags.ExcludeFiretrucks | GetEntitiesFlags.ExcludeAmbulances))
                    {
                        GameFiber.Yield();
                        if (AllCalloutEntities.Contains(veh))
                        {
                            continue;
                        }
                        if (veh != null)
                        {
                            if (veh.Exists())
                            {
                                if (veh != Game.LocalPlayer.Character.CurrentVehicle)
                                {
                                    if (!veh.CreatedByTheCallingPlugin)
                                    {
                                        if (!AllCalloutEntities.Contains(veh))
                                        {
                                            if (veh.Velocity.Length() > 0f)
                                            {
                                                Vector3 velocity = veh.Velocity;
                                                velocity.Normalize();
                                                velocity *= 0f;
                                                veh.Velocity = velocity;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            });
        }

        private void MakeNearbyPedsFlee()
        {
            GameFiber.StartNew(delegate
            {
                while (CalloutRunning)
                {

                    GameFiber.Yield();

                    foreach (Ped entity in World.GetEntities(SpawnPoint, 80f, GetEntitiesFlags.ConsiderAllPeds | GetEntitiesFlags.ExcludePlayerPed | GetEntitiesFlags.ExcludePoliceOfficers))
                    {
                        GameFiber.Yield();
                        if (AllCalloutEntities.Contains(entity))
                        {
                            continue;
                        }
                        if (entity != null)
                        {
                            if (entity.IsValid())
                            {

                                if (entity.Exists())
                                {
                                    if (entity != Game.LocalPlayer.Character)
                                    {
                                        if (entity != Game.LocalPlayer.Character.CurrentVehicle)
                                        {

                                            if (!entity.CreatedByTheCallingPlugin)
                                            {

                                                if (!AllCalloutEntities.Contains(entity))
                                                {
                                                    if (Vector3.Distance(entity.Position, SpawnPoint) < 74f)
                                                    {
                                                        if (entity.IsInAnyVehicle(false))
                                                        {
                                                            if (entity.CurrentVehicle != null)
                                                            {

                                                                entity.Tasks.PerformDrivingManeuver(VehicleManeuver.Wait);


                                                            }
                                                        }
                                                        else
                                                        {
                                                            Rage.Native.NativeFunction.CallByName<uint>("TASK_SMART_FLEE_COORD", entity, SpawnPoint.X, SpawnPoint.Y, SpawnPoint.Z, 75f, 6000, true, true);
                                                        }

                                                    }
                                                    if (Vector3.Distance(entity.Position, SpawnPoint) < 65f)
                                                    {
                                                        if (entity.IsInAnyVehicle(false))
                                                        {
                                                            if (entity.CurrentVehicle.Exists())
                                                            {
                                                                entity.CurrentVehicle.Delete();
                                                            }
                                                        }
                                                        if (entity.Exists())
                                                        {
                                                            entity.Delete();
                                                        }

                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            });
        }

        private void ClearUnrelatedEntities()
        {

            foreach (Ped entity in World.GetEntities(SpawnPoint, 50f, GetEntitiesFlags.ConsiderAllPeds))
            {
                GameFiber.Yield();
                if (entity != null)
                {
                    if (entity.IsValid())
                    {
                        if (entity.Exists())
                        {
                            if (entity != Game.LocalPlayer.Character)
                            {
                                if (entity != Game.LocalPlayer.Character.CurrentVehicle)
                                {
                                    if (!entity.CreatedByTheCallingPlugin)
                                    {

                                        if (!AllCalloutEntities.Contains(entity))
                                        {
                                            if (Vector3.Distance(entity.Position, SpawnPoint) < 50f)
                                            {
                                                entity.Delete();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            foreach (Vehicle entity in World.GetEntities(SpawnPoint, 50f, GetEntitiesFlags.ConsiderGroundVehicles))
            {
                GameFiber.Yield();
                if (entity != null)
                {
                    if (entity.IsValid())
                    {
                        if (entity.Exists())
                        {
                            if (entity != Game.LocalPlayer.Character)
                            {
                                if (entity != Game.LocalPlayer.Character.CurrentVehicle)
                                {
                                    if (!entity.CreatedByTheCallingPlugin)
                                    {

                                        if (!AllCalloutEntities.Contains(entity))
                                        {
                                            if (Vector3.Distance(entity.Position, SpawnPoint) < 50f)
                                            {
                                                entity.Delete();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void RobberSpawnWithEscape()
        {
            EscapeCar = new Vehicle("burrito", EscapeVehicle, EscapeVehicleFloat);
            EscapeCar.IsPersistent = true;
            EscapeCar.IsStolen = true;
            AllCalloutEntities.Add(EscapeCar);
            Ped EscapeCarDriver = new Ped("mp_g_m_pros_01", EscapeVehicle, EscapeVehicleFloat);
            EscapeCarDriver.WarpIntoVehicle(EscapeCar, -1);
            EscapeCarDriver.BlockPermanentEvents = true;
            EscapeCarDriver.IsPersistent = true;
            AllCalloutEntities.Add(EscapeCarDriver);
            Robbers.Add(EscapeCarDriver);
            Ped SafeCrackRobber = new Ped("mp_g_m_pros_01", RobberSafeCrack,0f);
            SafeCrackRobber.BlockPermanentEvents = true;
            SafeCrackRobber.IsPersistent = true;
            Robbers.Add(SafeCrackRobber);
            AllCalloutEntities.Add(SafeCrackRobber);
            Ped DoorWatch = new Ped("mp_g_m_pros_01", RobberWatchDoor, 180f);
            DoorWatch.BlockPermanentEvents = true;
            DoorWatch.IsPersistent = true;
            Robbers.Add(DoorWatch);
            AllCalloutEntities.Add(DoorWatch);
            Ped RobberAiming = new Ped("mp_g_m_pros_01", RobberSpawnPointAimingAtOwner, 0f);
            RobberAiming.BlockPermanentEvents = true;
            RobberAiming.IsPersistent = true;
            RobberAiming.Inventory.GiveNewWeapon("WEAPON_PISTOL50", 10000, true);
            Robbers.Add(RobberAiming);
            AllCalloutEntities.Add(RobberAiming);
            ShopOwner = new Ped("mp_m_shopkeep_01", ShopownerSpawn, ShopOwnerFloat);
            ShopOwner.BlockPermanentEvents = true;
            ShopOwner.IsPersistent = true;
            AllCalloutEntities.Add(ShopOwner);
            ShopOwner.Tasks.PlayAnimation("anim@heists@ornate_bank@hostages@hit", "hit_loop_ped_a", 1f, AnimationFlags.Loop | AnimationFlags.SecondaryTask);
            RobberAiming.Tasks.AimWeaponAt(ShopOwner, -1);
            foreach(Ped i in Robbers)
            {
                i.RelationshipGroup = "ROB";
                i.Inventory.GiveNewWeapon("WEAPON_PISTOL50", 10000, true);
            }
        }

        private void ShowOwnerKilledRobberSpawn()
        {
            Ped KilledRobber = new Ped("mp_g_m_pros_01", RobberSpawnPointAimingAtOwner, 0f);
            KilledRobber.IsPersistent = true;
            KilledRobber.BlockPermanentEvents = true;
            KilledRobber.Inventory.GiveNewWeapon("weapon_knife", 10000, true);
            Robbers.Add(KilledRobber);
            AllCalloutEntities.Add(KilledRobber);
            ShopOwner = new Ped("mp_m_shopkeep_01", ShopownerSpawn, ShopOwnerFloat);
            ShopOwner.Inventory.GiveNewWeapon("weapon_pumpshotgun", 1, true);
            ShopOwner.BlockPermanentEvents = true;
            ShopOwner.IsPersistent = true;
            AllCalloutEntities.Add(ShopOwner);
            ShopOwner.Face(KilledRobber);
            KilledRobber.Face(ShopOwner);
            KilledRobber.Inventory.EquippedWeapon.DropToGround();
            KilledRobber.Kill();
        }

        private void ShopOwnerCuffedSpawning()
        {
            ShopOwner = new Ped("mp_m_shopkeep_01", RobberSafeCrack, ShopOwnerFloat);
            ShopOwner.BlockPermanentEvents = true;
            ShopOwner.IsPersistent = true;
            AllCalloutEntities.Add(ShopOwner);
            ShopOwner.Tasks.PlayAnimation("rcmme_amanda1", "stand_loop_ama", 1f, AnimationFlags.Loop | AnimationFlags.SecondaryTask | AnimationFlags.UpperBodyOnly);
            EscapeCar = new Vehicle(Titles[new Random().Next(0, Titles.Length)], EscapeVehicle, EscapeVehicleFloat);
            EscapeCar.IsPersistent = true;
            AllCalloutEntities.Add(EscapeCar);
            Ped Robber = EscapeCar.CreateRandomDriver();
            Robber.IsPersistent = true;
            Robber.BlockPermanentEvents = true;
            Robbers.Add(Robber);
            AllCalloutEntities.Add(Robber);
            Functions.SetVehicleOwnerName(EscapeCar, Functions.GetPersonaForPed(Robber).FullName.ToString());
        }
        private void OfficerNeedsAssistanceSpawn()
        {
            ShopOwner = new Ped("mp_m_shopkeep_01", ShopownerSpawn, ShopOwnerFloat);
            ShopOwner.BlockPermanentEvents = true;
            ShopOwner.IsPersistent = true;
            AllCalloutEntities.Add(ShopOwner);
            EscapeCar = new Vehicle(CarModels[new Random().Next(0, CarModels.Length)], EscapeVehicle, EscapeVehicleFloat+180f);
            EscapeCar.IsPersistent = true;
            EscapeCar.IsSirenOn = true;
            EscapeCar.IsSirenSilent = true;
            AllCalloutEntities.Add(EscapeCar);
            AssistOfficer = new Ped(LSPDModels[new Random().Next(0, LSPDModels.Length)], RobberSafeCrack, 0f);
            Functions.SetPedAsCop(AssistOfficer);
            Functions.SetCopAsBusy(AssistOfficer, true);
            AssistOfficer.IsPersistent = true;
            AssistOfficer.BlockPermanentEvents = true;
            AllCalloutEntities.Add(AssistOfficer);
            Ped Robber = new Ped(RobberSafeCrack, AssistOfficer.Heading);
            Robber.Tasks.PlayAnimation("mp_arresting", "idle", 1f, AnimationFlags.Loop | AnimationFlags.SecondaryTask | AnimationFlags.UpperBodyOnly);
            Robber.IsPersistent = true;
            Robber.BlockPermanentEvents = true;
            Robbers.Add(Robber);
            AllCalloutEntities.Add(Robber);
        }

        private void GameEnd()
        {
            GameFiber.StartNew(delegate
            {
                GameFiber.Yield();
                if (Game.IsKeyDown(Settings.EndCalloutKey))
                {
                    this.End();
                }

            });
        }

        private void AttackRobbers()
        {
            GameFiber.StartNew(delegate
            {
                    if (Robbers.Any(rob => rob.DistanceTo(Game.LocalPlayer.Character) <= 3f || Game.LocalPlayer.Character.IsShooting))
                    {
                        foreach (Ped i in Robbers)
                        {
                            i.KeepTasks = true;
                            i.Tasks.FightAgainst(Game.LocalPlayer.Character, -1);
                        }
                        AttackingRobbers = true;
                    }
            });
        }

        private void ToggleReport(Ped ped, bool toggle)
        {

            if (toggle)
            {
                NativeFunction.Natives.SET_PED_CAN_SWITCH_WEAPON(ped, false);
                ped.Inventory.GiveNewWeapon(new WeaponAsset("WEAPON_UNARMED"), -1, true);
                Pencil = new Rage.Object(new Model("prop_pencil_01"), new Vector3(0, 0, 0));
                Paperwork = new Rage.Object(new Model("prop_fib_clipboard"), new Vector3(0, 0, 0));
                //int boneIndex = NativeFunction.Natives.GET_PED_BONE_INDEX<int>(ped, (int)PedBoneId.RightPhHand);
                int boneIndexPen = 90;
                int boneIndexClip = 42;
                Pencil.AttachTo(ped, boneIndexPen, new Vector3(-2.98023224e-08f, -0.0199999847f, 0f), new Rotator(-1.69873238e-05f, -7.03334808e-06f, -7.03334808e-06f));
                Paperwork.AttachTo(ped, boneIndexClip, new Vector3(0.190000013f, 0.049999997f, 0.0599999949f), new Rotator(-130f, 24.9999981f, 29f));
                //NativeFunction.Natives.ATTACH_ENTITY_TO_ENTITY(MobilePhone, ped, boneIndex, 0f, 0f, 0f, 0f, 0f, 0f, true, true, false, false, 2, 1);
                ped.Tasks.PlayAnimation("anim@amb@board_room@supervising@", "look_up_01_amy_skater_01", 1.3f, AnimationFlags.Loop | AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask);

            }
            else
            {
                NativeFunction.Natives.SET_PED_CAN_SWITCH_WEAPON(ped, true);
                ped.Tasks.Clear();
                if (GameFiber.CanSleepNow)
                {
                    GameFiber.Wait(800);
                }
                if (Pencil.Exists()) { Pencil.Delete(); }
                if (Paperwork.Exists()) { Paperwork.Delete(); }
            }
        }

        private void WatchCameraFootage()
        {
            Game.LocalPlayer.HasControl = false;
            Game.FadeScreenOut(1500, true);
            NativeFunction.Natives.SET_TIMECYCLE_MODIFIER("CAMERA_BW");
            if (Game.LocalPlayer.Character.LastVehicle.Exists())

            {
                Game.LocalPlayer.Character.LastVehicle.IsVisible = false;
            }
            
            Ped ShopKeeperClone = ShopOwner.Clone();
            ShopKeeperClone.IsPersistent = true;
            ShopKeeperClone.Position = ShopownerSpawn;
            AllCalloutEntities.Add(ShopKeeperClone);
            Ped KilledRobClone = Robbers.First().Clone();
            KilledRobClone.IsPersistent = true;
            KilledRobClone.Health = 100;
            AllCalloutEntities.Add(KilledRobClone);
            Camera cam = new Camera(true);
            cam.Position = CameraLocation;
            cam.Face(ShopKeeperClone);
            KilledRobClone.Position = SpawnPoint;
            KilledRobClone.Inventory.GiveNewWeapon("weapon_knife", 1, true);
            KilledRobClone.DropsCurrentWeaponOnDeath = false;
            GameFiber.Sleep(2000);
            Game.FadeScreenIn(1500, true);
            Game.DisplaySubtitle("~r~Robber~s~: Give me all your money, you cunt!");
            KilledRobClone.Tasks.FollowNavigationMeshToPosition(RobberSpawnPointAimingAtOwner, ShopKeeperClone.Heading+180f, 1f).WaitForCompletion();
            ShopKeeperClone.Inventory.GiveNewWeapon("weapon_pumpshotgun", 100, true);
            ShopKeeperClone.Tasks.FightAgainst(KilledRobClone);
            GameFiber.Sleep(6500);
            Game.FadeScreenOut(1500, true);
            if (Game.LocalPlayer.Character.LastVehicle.Exists())
            {
                Game.LocalPlayer.Character.LastVehicle.IsVisible = true;
            }
            AllCalloutEntities.Remove(ShopKeeperClone);
            AllCalloutEntities.Remove(KilledRobClone);
            ShopKeeperClone.Delete();
            KilledRobClone.Delete();
            Game.LocalPlayer.HasControl = true;
            cam.Delete();
            GameFiber.Sleep(2000);
            NativeFunction.CallByName<uint>("CLEAR_TIMECYCLE_MODIFIER");
            Game.FadeScreenIn(1500, true);

        }
    }

}
