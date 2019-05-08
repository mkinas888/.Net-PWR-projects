import javax.swing.*;
import javax.swing.table.DefaultTableModel;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.concurrent.ThreadLocalRandom;

class MainWindow extends JFrame {
    private JPanel panelMain;
    private JButton buttonTryMe;
    private static int xCord;
    private static int yCord;
    private static int width;
    private static int height;

   void  animateMove(){

    }

    MainWindow() {
        this.setSize(1000, 600);
        width = this.getWidth();
        height = this.getHeight();
        buttonTryMe.setPreferredSize(new Dimension(100, 50));
        buttonTryMe.setBorder(BorderFactory.createEmptyBorder(0,0,0,0));
        this.setTitle("Platformy programistyczne .Net i Java, labolatorium, Java, lab01");
        this.setContentPane(panelMain);
        buttonTryMe.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                try{
                    xCord = ThreadLocalRandom.current().nextInt(0, width - 100);
                    yCord = ThreadLocalRandom.current().nextInt(0, height - 50);
                    buttonTryMe.setLocation(xCord,yCord);
                } catch(NumberFormatException ex) {
                    JOptionPane.showMessageDialog(panelMain, "Something went wrong.");
                }
            }
        });
    }
}
