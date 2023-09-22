using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Netcode;
using StardewValley.Quests;
using StardewValley;
using Object = StardewValley.Object;
using RestStopLocations.Quests;
using StardewValley.Extensions;

namespace RestStopCode
{
    [XmlType("Mods_ApryllForever_RestStopCode_MermaidItemDeliveryQuest")]
    public class MermaidItemDeliveryQuest : ItemDeliveryQuest 
    {
        public string targetMessage;

        [XmlElement("target")]
        public readonly NetString target = new NetString();

        [XmlElement("buttitem")]
        public readonly NetString buttitem = new NetString();

        [XmlElement("number")]
        public readonly NetInt number = new NetInt(1);

        [XmlElement("deliveryItem")]
        public readonly NetRef<Object> deliveryItem = new NetRef<Object>();

        public readonly NetDescriptionElementList parts = new NetDescriptionElementList();

        public readonly NetDescriptionElementList dialogueparts = new NetDescriptionElementList();

        [XmlElement("objective")]
        public readonly NetDescriptionElementRef objective = new NetDescriptionElementRef();

        public bool isOksanaSmuggleQuest = false;

        public bool isOksanaCookingQuest = false;

        public MermaidItemDeliveryQuest()
        {
            base.questType.Value = 3;
        }

        protected override void initNetFields()
        {
            base.initNetFields();
            base.NetFields.AddField(this.target).AddField(this.buttitem).AddField(this.number).AddField(this.deliveryItem).AddField(this.parts).AddField(this.dialogueparts).AddField(this.objective);
        }

        public List<NPC> GetValidMermaidTargetList()
        {
            List<NPC> valid_targets = new List<NPC>();

            NPC vika = Game1.getCharacterFromName("Vika");
            NPC oksie = Game1.getCharacterFromName("Oksana");
            NPC collie = Game1.getCharacterFromName("Colleen");
            valid_targets.Add(oksie);
            valid_targets.Add(vika);
            valid_targets.Add(collie);

            //valid_targets.OrderBy((NPC n) => n.Name);

            return valid_targets;
        }

        new public void loadQuestInfo()
        {
            if (this.target.Value != null)
            {
                return;
            }
            base.questTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13285");
           
            List<NPC> valid_targets = this.GetValidMermaidTargetList();
            // this.target.Value = valid_targets[base.random.Next(valid_targets.Count)].Name;

            NPC actualTarget;

            actualTarget = valid_targets[base.random.Next(valid_targets.Count)];
            if (actualTarget == null)
            {
                return;
            }

            if (base.random.NextDouble() <= 0.33)
            {
                this.target.Value = valid_targets[0].Name;
                if ( base.random.NextDouble() < 0.5)
                {
                    this.isOksanaSmuggleQuest = true;

                    List<string> oksanaLoot = QuestDictionaries.OksanaSmuggleItems();
                    this.buttitem.Value = oksanaLoot.ElementAt(base.random.Next(oksanaLoot.Count));
                    this.deliveryItem.Value = (Object)ItemRegistry.Create(buttitem);
                    



                }

                else
                {
                    this.isOksanaCookingQuest = true;

                    List<string> oksanaLoot = QuestDictionaries.OksanaCookItems();
                    this.buttitem.Value = oksanaLoot.ElementAt(base.random.Next(oksanaLoot.Count));
                    this.deliveryItem.Value = (Object)ItemRegistry.Create(buttitem);
                    this.parts.Clear();

                    this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:Oksana.OksanaCookQuest.Description", this.deliveryItem.Value));
                    this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:Oksana.Signature"));

                }
            }

            else if (base.random.NextDouble() <= 0.67)
            {
                this.target.Value = valid_targets[1].Name;

                List<string> vikaLoot = QuestDictionaries.VikaItems();
                this.buttitem.Value = vikaLoot.ElementAt(base.random.Next(vikaLoot.Count));
                this.deliveryItem.Value = (Object)ItemRegistry.Create(buttitem);
                this.parts.Clear();

                /*
                this.item.Value = Utility.getRandomItemFromSeason(Game1.currentSeason, 1000, forQuest: true);

                this.deliveryItem.Value = new Object(Vector2.Zero, this.item, 1);
                DescriptionElement[] questDescriptions = null;
                DescriptionElement[] questDescriptions2 = null;
                DescriptionElement[] questDescriptions3 = null;
                if (Game1.objectInformation[this.item].Split('/')[3].Split(' ')[0].Equals("Cooking") && !this.target.Value.Equals("Wizard"))
                {
                    if (base.random.NextDouble() < 0.33)
                    {
                        DescriptionElement[] questStrings3 = new DescriptionElement[12]
                        {
                        "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13336",
                        "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13337",
                        "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13338",
                        "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13339",
                        "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13340",
                        "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13341",
                        (!Game1.samBandName.Equals(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2156"))) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13347", new DescriptionElement("Strings\\StringsFromCSFiles:Game1.cs.2156")) : ((!Game1.elliottBookName.Equals(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2157"))) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13342", new DescriptionElement("Strings\\StringsFromCSFiles:Game1.cs.2157")) : ((DescriptionElement)"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13346")),
                        "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13349",
                        "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13350",
                        "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13351",
                        Game1.currentSeason.Equals("winter") ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13353" : (Game1.currentSeason.Equals("summer") ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13355" : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13356"),
                        "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13357"
                        };
                        this.parts.Clear();
                        this.parts.Add((base.random.NextDouble() < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13333", this.deliveryItem.Value, questStrings3.ElementAt(base.random.Next(12))) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13334", this.deliveryItem.Value, questStrings3.ElementAt(base.random.Next(12))));
                        this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", actualTarget));
                    }
                    else
                    {
                        DescriptionElement day = new DescriptionElement();
                        switch (Game1.dayOfMonth % 7)
                        {
                            case 0:
                                day = "Strings\\StringsFromCSFiles:Game1.cs.3042";
                                break;
                            case 1:
                                day = "Strings\\StringsFromCSFiles:Game1.cs.3043";
                                break;
                            case 2:
                                day = "Strings\\StringsFromCSFiles:Game1.cs.3044";
                                break;
                            case 3:
                                day = "Strings\\StringsFromCSFiles:Game1.cs.3045";
                                break;
                            case 4:
                                day = "Strings\\StringsFromCSFiles:Game1.cs.3046";
                                break;
                            case 5:
                                day = "Strings\\StringsFromCSFiles:Game1.cs.3047";
                                break;
                            case 6:
                                day = "Strings\\StringsFromCSFiles:Game1.cs.3048";
                                break;
                        }
                        questDescriptions = new DescriptionElement[5]
                        {
                        new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13360", this.deliveryItem.Value),
                        new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13364", this.deliveryItem.Value),
                        new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13367", this.deliveryItem.Value),
                        new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13370", this.deliveryItem.Value),
                        new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13373", day, this.deliveryItem.Value, actualTarget)
                        };
                        questDescriptions2 = new DescriptionElement[5]
                        {
                        new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", actualTarget),
                        new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", actualTarget),
                        new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", actualTarget),
                        new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", actualTarget),
                        ""
                        };
                        questDescriptions3 = new DescriptionElement[5] { "", "", "", "", "" };
                    }
                    this.parts.Clear();
                    int rand5 = base.random.Next(questDescriptions.Count());
                    this.parts.Add(questDescriptions[rand5]);
                    this.parts.Add(questDescriptions2[rand5]);
                    this.parts.Add(questDescriptions3[rand5]);

                }
                else if (base.random.NextDouble() < 0.5 && Convert.ToInt32(Game1.objectInformation[this.item].Split('/')[2]) > 0)
                {
                    questDescriptions = new DescriptionElement[2]
                    {
                    new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13383", this.deliveryItem.Value, new DescriptionElement[12]
                    {
                        "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13385", "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13386", "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13387", "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13388", "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13389", "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13390", "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13391", "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13392", "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13393", "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13394",
                        "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13395", "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13396"
                    }.ElementAt(base.random.Next(12))),
                    new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13400", this.deliveryItem.Value)
                    };
                    questDescriptions2 = new DescriptionElement[2]
                    {
                    new DescriptionElement((base.random.NextDouble() < 0.5) ? "" : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13398"),
                    new DescriptionElement((base.random.NextDouble() < 0.5) ? "" : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13402")
                    };
                    questDescriptions3 = new DescriptionElement[2]
                    {
                    new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", actualTarget),
                    new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", actualTarget)
                    };
                    if (base.random.NextDouble() < 0.33)
                    {
                        DescriptionElement[] questSTrings = new DescriptionElement[12]
                        {
                        "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13336",
                        "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13337",
                        "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13338",
                        "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13339",
                        "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13340",
                        "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13341",
                        (!Game1.samBandName.Equals(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2156"))) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13347", new DescriptionElement("Strings\\StringsFromCSFiles:Game1.cs.2156")) : ((!Game1.elliottBookName.Equals(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2157"))) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13342", new DescriptionElement("Strings\\StringsFromCSFiles:Game1.cs.2157")) : ((DescriptionElement)"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13346")),
                        "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13420",
                        "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13421",
                        "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13422",
                        Game1.currentSeason.Equals("winter") ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13424" : (Game1.currentSeason.Equals("summer") ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13426" : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13427"),
                        "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13357"
                        };
                        this.parts.Clear();
                        this.parts.Add((base.random.NextDouble() < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13333", this.deliveryItem.Value, questSTrings.ElementAt(base.random.Next(12))) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13334", this.deliveryItem.Value, questSTrings.ElementAt(base.random.Next(12))));
                        this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", actualTarget));
                    }
                    else
                    {
                        this.parts.Clear();
                        int rand4 = base.random.Next(questDescriptions.Count());
                        this.parts.Add(questDescriptions[rand4]);
                        this.parts.Add(questDescriptions2[rand4]);
                        this.parts.Add(questDescriptions3[rand4]);
                    }

                }
                else if (base.random.NextDouble() < 0.5 && Convert.ToInt32(Game1.objectInformation[this.item].Split('/')[2]) < 0)
                {
                    this.parts.Clear();
                    this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13464", this.deliveryItem.Value, new DescriptionElement[5] { "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13465", "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13466", "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13467", "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13468", "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13469" }.ElementAt(base.random.Next(5))));
                    this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", actualTarget));
                    if (this.target.Value.Equals("Emily"))
                    {
                        this.parts.Clear();
                        this.parts.Add((base.random.NextDouble() < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13473", this.deliveryItem.Value) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13476", this.deliveryItem.Value));
                    }
                }
                else
                {
                    DescriptionElement[] questStrings = new DescriptionElement[12]
                    {
                    "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13502", "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13503", "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13504", "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13505", "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13506", "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13507", "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13508", "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13509", "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13510", "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13511",
                    "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13512", "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13513"
                    };
                    questDescriptions = new DescriptionElement[9]
                    {
                    new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13480", actualTarget, this.deliveryItem.Value),
                    new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13481", this.deliveryItem.Value),
                    new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13485", this.deliveryItem.Value),
                    (base.random.NextDouble() < 0.4) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13491", this.deliveryItem.Value) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13492", this.deliveryItem.Value),
                    new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13494", this.deliveryItem.Value),
                    new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13497", this.deliveryItem.Value),
                    new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13500", this.deliveryItem.Value, questStrings.ElementAt(base.random.Next(12))),
                    new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13518", actualTarget, this.deliveryItem.Value),
                    (base.random.NextDouble() < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13520", this.deliveryItem.Value) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13523", this.deliveryItem.Value)
                    };
                    questDescriptions2 = new DescriptionElement[9]
                    {
                    "",
                    (base.random.NextDouble() < 0.3) ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13482" : ((base.random.NextDouble() < 0.5) ? "" : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13483"),
                    (base.random.NextDouble() < 0.25) ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13487" : ((base.random.NextDouble() < 0.33) ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13488" : ((base.random.NextDouble() < 0.5) ? "" : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13489")),
                    new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", actualTarget),
                    new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", actualTarget),
                    new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", actualTarget),
                    (base.random.NextDouble() < 0.5) ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13514" : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13516",
                    "",
                    new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", actualTarget)
                    };
                    questDescriptions3 = new DescriptionElement[9]
                    {
                    "",
                    new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", actualTarget),
                    new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", actualTarget),
                    "",
                    "",
                    "",
                    new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", actualTarget),
                    "",
                    ""
                    };
                    this.parts.Clear();
                    int rand3 = base.random.Next(questDescriptions.Count());
                    this.parts.Add(questDescriptions[rand3]);
                    this.parts.Add(questDescriptions2[rand3]);
                    this.parts.Add(questDescriptions3[rand3]);
                }
                    */
            
            }

            else
            {
                this.target.Value = valid_targets[2].Name; //Colleen
               
                {
                    List<string> colleenLoot = QuestDictionaries.ColleenItems();
                    this.buttitem.Value = colleenLoot.ElementAt(base.random.Next(colleenLoot.Count));
                    this.deliveryItem.Value = (Object)ItemRegistry.Create(buttitem);
                    this.parts.Clear();

                    this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:Colleen.DeliveryQuest.Request", this.deliveryItem.Value));
                    //this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:Oksana.Signature"));
                }
            }

            this.dialogueparts.Clear();
           
            this.dialogueparts.Add((base.random.NextBool(0.3) || this.target.Value == "Evelyn") ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13526") : (base.random.NextBool() ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13527") : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13528", Game1.player.Name)));

            this.dialogueparts.Add(base.random.NextBool(0.3) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13530", buttitem) : (base.random.NextBool() ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13532") : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs." + base.random.Choose("13534", "13535", "13536"))));
            this.dialogueparts.Add((base.random.NextDouble() < 0.3) ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13538" : ((base.random.NextDouble() < 0.5) ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13539" : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13540"));
            this.dialogueparts.Add((base.random.NextDouble() < 0.3) ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13542" : ((base.random.NextDouble() < 0.5) ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13543" : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13544"));
            

            
            if (this.target.Value.Equals("Oksana")) //Haley
            {

                if (isOksanaSmuggleQuest == true && isOksanaCookingQuest == false)
                {
                    this.parts.Clear();
                    this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:Oksana.OksanaQuest.Description", this.deliveryItem.Value));
                    this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:Oksana.Signature"));
                }

                else if (isOksanaCookingQuest == true && isOksanaSmuggleQuest == false)
                {
                    this.parts.Clear();
                    this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:Oksana.OksanaCookQuest.Description", this.deliveryItem.Value));
                    this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:Oksana.Signature"));
                }


                //this.parts.Add((base.random.NextDouble() < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13557", this.deliveryItem.Value) : (Game1.player.isMale ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13560", this.deliveryItem.Value) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13563", this.deliveryItem.Value)));
                this.dialogueparts.Clear();
                this.dialogueparts.Add("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13566");


            }

            if (this.target.Value.Equals("Colleen")) //Maru
            {
                this.parts.Clear();
                this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:Colleen.DeliveryQuest.Request", this.deliveryItem.Value));


                //double rand2 = base.random.NextDouble();
                //this.parts.Add((rand2 < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13580", this.deliveryItem.Value) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13583", this.deliveryItem.Value));
                this.dialogueparts.Clear();
                this.dialogueparts.Add("Strings\\StringsFromCSFiles:Colleen.DeliveryQuest.Delivered");

                //this.dialogueparts.Add((rand2 < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13585", Game1.player.Name) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13587", Game1.player.Name));
            }
            if (this.target.Value.Equals("Vika")) //Abigail
            {
                this.parts.Clear();
                double rand = base.random.NextDouble();
                this.parts.Add((rand < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13590", this.deliveryItem.Value) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13593", this.deliveryItem.Value));
                this.dialogueparts.Add((rand < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13597", Game1.player.Name) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13599", Game1.player.Name));
            }  

            DescriptionElement lastPart = ((base.random.NextDouble() < 0.3) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13608", actualTarget) : ((!(base.random.NextDouble() < 0.5)) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13612", actualTarget) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13610", actualTarget)));

            lastPart = new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs." + base.random.Choose("13608", "13610", "13612"), actualTarget);



            if (isOksanaSmuggleQuest == true)
                this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13607", (int)this.deliveryItem.Value.price * 10));

            else if (isOksanaSmuggleQuest == true)
                this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13607", (int)this.deliveryItem.Value.price * 7));

            else
                this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13607", (int)this.deliveryItem.Value.price * 4));


            this.parts.Add(lastPart);
            this.objective.Value = new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13614", this.target.Value, this.deliveryItem.Value);
        
        
        
        }

        public override void reloadDescription()
        {
            if (base._questDescription == "")
            {
                this.loadQuestInfo();
            }
            string descriptionBuilder = "";
            string messageBuilder = "";
            if (this.parts != null && this.parts.Count != 0)
            {
                foreach (DescriptionElement a in this.parts)
                {
                    descriptionBuilder += a.loadDescriptionElement();
                }
                base.questDescription = descriptionBuilder;
            }
            if (this.dialogueparts != null && this.dialogueparts.Count != 0)
            {
                foreach (DescriptionElement b in this.dialogueparts)
                {
                    messageBuilder += b.loadDescriptionElement();
                }
                this.targetMessage = messageBuilder;
            }
            else
            {
                if ((string)base.id == "0")
                {
                    return;
                }
                Dictionary<string, string> questData = Game1.temporaryContent.Load<Dictionary<string, string>>("Data\\Quests");
                if (questData != null && questData.ContainsKey(base.id))
                {
                    string[] rawData = questData[base.id].Split('/');
                    if (rawData != null && rawData.Length >= 9)
                    {
                        this.targetMessage = rawData[9];
                    }
                }
            }
        }

        public override void reloadObjective()
        {
            if (this.objective.Value != null)
            {
                base.currentObjective = this.objective.Value.loadDescriptionElement();
            }
        }

        public override bool checkIfComplete(NPC n = null, int number1 = -1, int number2 = -1, Item item = null, string monsterName = null, bool probe = false)
        {
            if ((bool)base.completed)
            {
                return false;
            }
            if (n != null && n.isVillager() && n.Name == this.target.Value && item?.QualifiedItemId == this.ItemId.Value)
            {
                if (item.Stack >= (int)this.number.Value)
                {
                    Game1.player.ActiveObject.Stack -= (int)this.number.Value - 1;
                    this.reloadDescription();
                    n.CurrentDialogue.Push(new Dialogue(n, null, this.targetMessage));
                    Game1.drawDialogue(n);
                    Game1.player.reduceActiveItemByOne();
                    if ( isOksanaSmuggleQuest == true)
                    {
                        Game1.player.changeFriendship(50, n);
                        if (this.deliveryItem.Value == null)
                        {
                                   this.deliveryItem.Value = (Object)item;
                        }
                        base.moneyReward.Value = (int)this.deliveryItem.Value.price * 10;
                    }
                    else if (isOksanaCookingQuest == true )
                    {
                        Game1.player.changeFriendship(50, n);
                        if (this.deliveryItem.Value == null)
                        {
                            this.deliveryItem.Value = (Object)item;
                        }
                        base.moneyReward.Value = (int)this.deliveryItem.Value.price * 7;
                    }
                    else
                    {
                        Game1.player.changeFriendship(50, n);
                        if (this.deliveryItem.Value == null)
                        {
                            this.deliveryItem.Value = (Object)item;
                        }
                        base.moneyReward.Value = (int)this.deliveryItem.Value.price * 4;
                    }
                    this.questComplete();
                    return true;
                }
                n.CurrentDialogue.Push(Dialogue.FromTranslation(n, "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13615", this.number.Value));
                Game1.drawDialogue(n);
                return false;
            }
            return false;
        }
    }
}