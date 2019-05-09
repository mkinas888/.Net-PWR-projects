import javax.swing.*;
import javax.swing.table.DefaultTableModel;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.concurrent.ThreadLocalRandom;

import static java.lang.Math.min;

class MainWindow extends JFrame {
    private JPanel panelMain;
    private JButton buttonTryMe;
    private  int xCord;
    private  int yCord;
    private  int width;
    private  int height;
    private  Point buttonLocation;
    private long initialTime;
    private double distance;
    private double angle;


    MainWindow() {
        Toolkit tk = Toolkit.getDefaultToolkit();
        width = tk.getScreenSize().width;
        height = tk.getScreenSize().height;
        System.out.println(width);
        buttonTryMe.setPreferredSize(new Dimension(100, 50));
        buttonTryMe.setBorder(BorderFactory.createEmptyBorder(0,0,0,0));
        this.setTitle("Platformy programistyczne .Net i Java, labolatorium, Java, lab01");
        this.setContentPane(panelMain);
        buttonTryMe.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                try{
                    xCord = ThreadLocalRandom.current().nextInt(0, width - 100);
                    yCord = ThreadLocalRandom.current().nextInt(0, height - 50);
                    buttonLocation = buttonTryMe.getLocation();
                    initialTime = System.nanoTime();
                    double newX = buttonLocation.x - xCord;
                    double newY = buttonLocation.y - yCord;
                    distance = Math.sqrt(newX * newX + newY * newY);
                    angle = Math.toDegrees(Math.atan2(newY, newX));
                    Timer timer = new Timer(1000 / 20, new ActionListener() {
                        @Override
                        public void actionPerformed(ActionEvent e) {
                            double duration = (System.nanoTime() - initialTime) / 1e6;
                            int speed = 50;
                            double distanceSoFar = min(speed * duration / 1000d, distance);
                            int x = buttonLocation.x - (int) (distanceSoFar * Math.cos(Math.toRadians(angle)));
                            int y = buttonLocation.y - (int) (distanceSoFar * Math.sin(Math.toRadians(angle)));
                            buttonTryMe.setLocation(x,y);
                        }
                    });
                    timer.setRepeats(true);
                    timer.start();
                } catch(NumberFormatException ex) {
                    JOptionPane.showMessageDialog(panelMain, "Something went wrong.");
                }
            }
        });
    }
}
