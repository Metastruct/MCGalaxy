﻿/*
    Copyright 2015 MCGalaxy
    
    Dual-licensed under the Educational Community License, Version 2.0 and
    the GNU General Public License, Version 3 (the "Licenses"); you may
    not use this file except in compliance with the Licenses. You may
    obtain a copy of the Licenses at

    http://www.opensource.org/licenses/ecl2.php
    http://www.gnu.org/licenses/gpl-3.0.html

    Unless required by applicable law or agreed to in writing,
    software distributed under the Licenses are distributed on an "AS IS"
    BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
    or implied. See the Licenses for the specific language governing
    permissions and limitations under the Licenses.
 */

namespace MCGalaxy.Commands {    
    public abstract class EntityPropertyCmd : Command {        
        public override bool museumUsable { get { return true; } }
        
        protected void UseBotOrPlayer(Player p, string message, string type) {
            if (message == "") { Help(p); return; }
            bool isBot = message.CaselessStarts("bot ");            
            string[] args = message.SplitSpaces(isBot ? 3 : 2);
            if (!CheckOwn(p, args, "player or bot name")) return;
            
            Player who = null;
            PlayerBot bot = null;            
            if (isBot) bot = PlayerBot.FindMatchesPreferLevel(p, args[1]);
            else who = PlayerInfo.FindMatches(p, args[0]);
            if (bot == null && who == null) return;
            
            if (p != null && who != null && who.Rank > p.Rank) {
                MessageTooHighRank(p, "change the " + type + " of", true); return;
            }
            if ((isBot || p != who) && !CheckExtraPerm(p)) { MessageNeedExtra(p, "change the " + type + " of others."); return; }
            if (isBot) SetBotData(p, bot, args);
            else SetPlayerData(p, who, args);
        }
        
        protected void UsePlayer(Player p, string message, string type) {
            if (message == "") { Help(p); return; }
            string[] args = message.SplitSpaces(2);
            if (!CheckOwn(p, args, "player name")) return;
            
            Player who = PlayerInfo.FindMatches(p, args[0]);
            if (who == null) return;
            
            if (p != null && who.Rank > p.Rank) {
                MessageTooHighRank(p, "change the " + type + " of", true); return;
            }
            if (p != who && !CheckExtraPerm(p)) { MessageNeedExtra(p, "change the " + type + " of others."); return; }
            SetPlayerData(p, who, args);
        }
        
        bool CheckOwn(Player p, string[] args, string type) {
            if (args[0].CaselessEq("-own")) {
                if (Player.IsSuper(p)) { SuperRequiresArgs(p, type); return false; }
                args[0] = p.name;
            }
            return true;
        }

        protected virtual void SetBotData(Player p, PlayerBot bot, string[] args) { }
        
        protected virtual void SetPlayerData(Player p, Player who, string[] args) { }
    }
}