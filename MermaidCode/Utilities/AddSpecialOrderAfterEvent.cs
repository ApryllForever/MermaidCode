﻿using System;
using StardewModdingAPI;
using StardewValley;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using StardewModdingAPI.Events;
using StardewValley.SpecialOrders;

namespace RestStopCode
{
    /// <summary>Adds specific <see cref="SpecialOrder"/>s to all players' quest logs after viewing specific events.</summary>
    public static class AddSpecialOrdersAfterEvents
    {
        /// <summary>True if this class's features are currently enabled.</summary>
        public static bool Enabled { get; private set; } = false;
        /// <summary>The SMAPI helper instance to use for events and other API access.</summary>
        private static IModHelper Helper { get; set; } = null;
        /// <summary>The monitor instance to use for log messages. Null if not provided.</summary>
        private static IMonitor Monitor { get; set; } = null;

        
        public static void Enable(IModHelper helper, IMonitor monitor)
        {
            if (!Enabled && helper != null && monitor != null) 
            {
                Helper = helper; 
                Monitor = monitor; 

                Helper.Events.GameLoop.UpdateTicked += CurrentEventEnded_UpdateSpecialOrders;
                Helper.Events.GameLoop.DayStarted += DayStarted_UpdateSpecialOrders;

                Enabled = true;
            }
        }


        /****************/
        /* Mod Settings */
        /****************/


        /// <summary>A list of special orders and the conditions required for them to appear in the players' quest logs.</summary>
        /// <remarks>Orders will be automatically added if the conditions are all met, and automatically removed if they are unmet.</remarks>
        public static List<SpecialOrderConditions> SpecialOrders = new List<SpecialOrderConditions>()
        {
            new SpecialOrderConditions()
            {
                OrderKey = "VikaBug1",
                HasSeenEvents = "11429005",
                HasNotSeenEvents = "11429007"
            },
             new SpecialOrderConditions()
            {
                OrderKey = "VikaBug2",
                HasSeenEvents = "11429702",
                HasNotSeenEvents = "11429703"
            },


        };


        /*****************/
        /* Internal Code */
        /*****************/


        /// <summary>A special order key and the conditions required for it to appear in players' quest logs.</summary>
        public class SpecialOrderConditions
        {
            public string OrderKey { get; set; } = null;
            public string HasSeenEvents { get; set; } = null;
            public string HasNotSeenEvents { get; set; } = null;
        }

        /// <summary>Converts a string of event IDs (e.g. "111 222 333") into a list of integers.</summary>
        /// <param name="eventsString">The string of event IDs to convert. Events may be separated by any of number of spaces and/or commas.</param>
        /// <returns>A list of integer event IDs.</returns>
        public static List<string> ParseEventsString(string eventsString)
        {
            List<string> events = new List<string>(); //create a list of event IDs

            if (eventsString != null) //if a string of events was provided
            {
                foreach (string entry in eventsString.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries)) //for each individual event ID within the string (separated by spaces/commas)
                {
                    events.Add(entry); //convert the ID to an integer and add it to the list
                }
            }

            return events; //return the completed list
        }

        /// <summary>The active event ID during the previous tick.</summary>
        private static string? lastEventID = null;

        /// <summary>Updates players' special orders whenever an in-game event ends.</summary>
        private static void CurrentEventEnded_UpdateSpecialOrders(object sender, UpdateTickedEventArgs e)
        {
            string? currentEventID = Game1.CurrentEvent?.id; //get the current quest ID (if any)

            if (currentEventID == null) //if an event just ended
            {
              if (lastEventID != null)
                {
                    UpdateSpecialOrders();
                }
            }

            lastEventID = currentEventID; //update recorded event ID
        }

        /// <summary>Updates players' special orders whenever a new in-game day begins.</summary>
        private static void DayStarted_UpdateSpecialOrders(object sender, DayStartedEventArgs e)
        {
            UpdateSpecialOrders();
        }

        /// <summary>Updates players' quest logs to add/remove each entry in <see cref="SpecialOrders"/>, depending on whether their conditions are met.</summary>
        private static void UpdateSpecialOrders()
        {
            foreach (var entry in SpecialOrders) //for each entry in the special orders list
            {
                List<string> seenEvents;
                List<string> notSeenEvents;

                
                try
                {
                    //try to parse this order's conditions
                    seenEvents =        ParseEventsString(entry.HasSeenEvents);
                    notSeenEvents = ParseEventsString(entry.HasNotSeenEvents);
                }
                catch (Exception ex) //if the conditions couldn't be parsed
                {
                    Monitor.Log($"Failed to parse event ID lists for this special order: \"{entry.OrderKey}\". The order won't be added/removed until this error is fixed. Full error message: \n{ex.ToString()}", LogLevel.Error);
                    continue; //skip to the next order
                }  

                bool allConditionsMet = true; //true if players meet all the required conditions for this order

                //prepare log message data
                string unmetSeenEvents = "";
                string unmetNotSeenEvents = "";

                foreach (string seenEvent in seenEvents) //for each event the players must have seen
                {
                    if (Game1.getAllFarmers().Any(farmer => farmer.eventsSeen.Contains(seenEvent)) == false) //if NO players have seen this event
                    {
                        unmetSeenEvents += $" {seenEvent}";
                        allConditionsMet = false;
                    }
                }

                foreach (string notSeenEvent in notSeenEvents) //for each event the players must NOT have seen
                {
                    if (Game1.getAllFarmers().Any(farmer => farmer.eventsSeen.Contains(notSeenEvent)) == true) //if any player has seen this event
                    {
                        unmetNotSeenEvents += $" {notSeenEvent}";
                        allConditionsMet = false;
                    }
                }

                if (allConditionsMet && Game1.player.team.completedSpecialOrders.ContainsKey(entry.OrderKey) == false) //if conditions are met AND the players have NOT completed this order
                {
                    if (Game1.player.team.SpecialOrderActive(entry.OrderKey) == false) //if the players do not already have this order
                    {
                        Monitor.Log($"Adding special order \"{entry.OrderKey}\" to quest logs. All conditions met; order has not been completed yet.", LogLevel.Trace);
                        SpecialOrder order = SpecialOrder.GetSpecialOrder(entry.OrderKey, null); //create this order
                        Game1.player.team.specialOrders.Add(order); //add it to the players' quest logs
                    }
                }
                else 
                {
                    for (int x = Game1.player.team.specialOrders.Count - 1; x >= 0; x--) //for each of the players' special orders (looping backward for easier removal)
                    {
                        SpecialOrder order = Game1.player.team.specialOrders[x]; //get the current order
                        if (order.questKey.Value.Equals(entry.OrderKey) //if this is the same order
                         && order.questState.Value == SpecialOrderStatus.InProgress) //AND this order is currently active
                        {
                            Monitor.Log($"Removing special order \"{entry.OrderKey}\" from quest logs. Reason(s):", LogLevel.Trace);
                            if (unmetSeenEvents?.Length > 0 || unmetNotSeenEvents.Length > 0) //if any conditions were unmet
                            {
                                Monitor.Log($"  Unmet \"HasSeenEvent\" conditions:{unmetSeenEvents}", LogLevel.Trace);
                                Monitor.Log($"  Unmet \"HasNotSeenEvent\" conditions:{unmetNotSeenEvents}", LogLevel.Trace);
                            }
                            else //if no unmet conditions were documented (i.e. this was removed because it was completed already)
                                Monitor.Log($"  All conditions met; order may have already been completed.", LogLevel.Trace);

                            order.OnFail();
                            Game1.player.team.specialOrders.RemoveAt(x); 
                        }
                    }
                }
            }
        }
    }
}