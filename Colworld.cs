using Godot;

public class Colworld : Node2D
{

    public void onPrincessBodyEntered(PhysicsBody2D body)
    {
        if (body.GetName().Equals("player"))
        {
            Label youWin = GetNode("youwin") as Label;
            youWin.Show();
        }
    }

}
