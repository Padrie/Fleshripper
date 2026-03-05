public class Earthquake : Spell
{ 
       
       
       public override void OnPlayerCollision(Player player)
       {
              player.TakeDamage(damage);
       }
}