import javax.swing.*;
import java.awt.*;

public class App{

    public static void main(String[] args){
        EventQueue.invokeLater(new Runnable(){
            public void run() {
                MainWindow frame = new MainWindow();
                frame.setUndecorated(true);
                frame.setBackground(new Color(1.0f,1.0f,1.0f,0.0f));
                frame.setExtendedState(frame.getExtendedState() | JFrame.MAXIMIZED_BOTH);
                frame.setVisible(true);
            }
        });
    }
}