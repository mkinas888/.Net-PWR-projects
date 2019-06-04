import java.awt.Color;
import java.awt.Dimension;
import java.awt.Font;
import java.awt.FontMetrics;
import java.awt.Graphics;
import javax.swing.ImageIcon;
import java.awt.Image;
import java.awt.Toolkit;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.KeyAdapter;
import java.awt.event.KeyEvent;
import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.TimeUnit;
import javax.swing.JPanel;
import javax.swing.Timer;

public class GameBoard extends JPanel implements ActionListener {

    private final int BOARD_WIDTH = 600;
    private final int BOARD_HEIGHT = 600;
    private final int DOT_SIZE = 25;
    private final int ALL_DOTS = (BOARD_WIDTH*BOARD_HEIGHT)/(DOT_SIZE*DOT_SIZE);
    private final int DELAY = 150;
    private final int ANIMATION = 2;

    private final int x[] = new int[ALL_DOTS];
    private final int y[] = new int[ALL_DOTS];

    private int dots;
    private int apple_x;
    private int apple_y;

    private List<Integer> foodPos = new ArrayList<Integer>();
    private int frameNumber = 0;

    private boolean leftDirection = false;
    private boolean rightDirection = true;
    private boolean upDirection = false;
    private boolean downDirection = false;
    private boolean inGame = true;

    private Timer timer;
    private Image ball;
    private Image red_ball;
    private Image apple;
    private Image head;

    public GameBoard() {

        initBoard();
    }

    private void initBoard() {

        addKeyListener(new TAdapter());
        setBackground(Color.black);
        setFocusable(true);

        setPreferredSize(new Dimension(BOARD_WIDTH, BOARD_HEIGHT));
        loadImages();
        initGame();
    }

    private void loadImages() {

        ball = new ImageIcon("src/resources/dot.png").getImage().getScaledInstance(DOT_SIZE, DOT_SIZE, Image.SCALE_DEFAULT);

        red_ball = new ImageIcon("src/resources/reddot.png").getImage().getScaledInstance(DOT_SIZE, DOT_SIZE, Image.SCALE_DEFAULT);

        apple = new ImageIcon("src/resources/fruit.png").getImage().getScaledInstance(DOT_SIZE, DOT_SIZE, Image.SCALE_DEFAULT);

        head = new ImageIcon("src/resources/head.png").getImage().getScaledInstance(DOT_SIZE, DOT_SIZE, Image.SCALE_DEFAULT);
    }

    private void initGame() {

        dots = 3;

        for (int z = 0; z < dots; z++) {
            x[z] = 50 - z * 10;
            y[z] = 50;
        }

        locateApple();

        timer = new Timer(DELAY, this);
        timer.start();
    }

    @Override
    public void paintComponent(Graphics g) {
        super.paintComponent(g);

        doDrawing(g);
    }

    private void doDrawing(Graphics g) {

        for (int i = 0; i < BOARD_HEIGHT ; i +=DOT_SIZE){
            for (int j = 0; j < BOARD_WIDTH; j += DOT_SIZE){
                g.drawRect(i, j, DOT_SIZE, DOT_SIZE);
            }
        }

        if (inGame) {
                g.drawImage(apple, apple_x, apple_y, this);

                for (int z = 0; z < dots; z++) {
                    if (z == 0) {
                        g.drawImage(head, x[z], y[z], this);
                    } else {
                    if(foodPos.size()  > 0) {
                        ++frameNumber;
                        for (int i = 0; i < foodPos.size(); i++) {
                            int tmp = foodPos.get(i);
                            if (tmp != z)
                                g.drawImage(ball, x[z], y[z], this);
                            else {
                                g.drawImage(red_ball, x[z], y[z], this);
                                if (frameNumber > 5) {
                                    foodPos.set(i, ++tmp);
                                    if (tmp > dots) foodPos.remove(i);
                                    frameNumber = 0;
                                }
                            }
                        }
                    }
                    else
                        g.drawImage(ball, x[z], y[z], this);
                    }
                }
            Toolkit.getDefaultToolkit().sync();


        } else {

            gameOver(g);
        }
    }

    private void gameOver(Graphics g) {

        String msg = "Game Over";
        Font small = new Font("Helvetica", Font.BOLD, 14);
        FontMetrics metr = getFontMetrics(small);

        g.setColor(Color.white);
        g.setFont(small);
        g.drawString(msg, (BOARD_WIDTH - metr.stringWidth(msg)) / 2, BOARD_HEIGHT / 2);
    }

    private void checkApple() {

        if ((x[0] == apple_x) && (y[0] == apple_y)) {

            dots++;
            locateApple();
            foodPos.add(1);
        }
    }

    private void move() {

        for (int z = dots; z > 0; z--) {
            x[z] = x[(z - 1)];
            y[z] = y[(z - 1)];
        }

        if (leftDirection) {
            x[0] -= DOT_SIZE;
        }

        if (rightDirection) {
            x[0] += DOT_SIZE;
        }

        if (upDirection) {
            y[0] -= DOT_SIZE;
        }

        if (downDirection) {
            y[0] += DOT_SIZE;
        }
    }

    private void checkCollision() {

        for (int z = dots; z > 0; z--) {

            if ((z > 4) && (x[0] == x[z]) && (y[0] == y[z])) {
                inGame = false;
            }
        }

        if (y[0] >= BOARD_HEIGHT) {
            inGame = false;
        }

        if (y[0] < 0) {
            inGame = false;
        }

        if (x[0] >= BOARD_WIDTH) {
            inGame = false;
        }

        if (x[0] < 0) {
            inGame = false;
        }

        if (!inGame) {
            timer.stop();
        }
    }

    private void locateApple() {

        boolean isNotInSnake;
        do {
            isNotInSnake = true;
            int r = (int) (Math.random() * (BOARD_WIDTH/DOT_SIZE));
            apple_x = ((r * DOT_SIZE));

            r = (int) (Math.random() * (BOARD_HEIGHT/DOT_SIZE));
            apple_y = ((r * DOT_SIZE));

            for (int z = 0; z< dots; z++){
                if(x[z] == apple_x && y[z] == apple_y)
                    isNotInSnake = false;
            }
        } while(!isNotInSnake);
    }

    @Override
    public void actionPerformed(ActionEvent e) {

        if (inGame) {

            checkApple();
            checkCollision();
            move();
        }

        repaint();
    }

    private class TAdapter extends KeyAdapter {

        @Override
        public void keyPressed(KeyEvent e) {

            int key = e.getKeyCode();

            if ((key == KeyEvent.VK_LEFT) && (!rightDirection)) {
                leftDirection = true;
                upDirection = false;
                downDirection = false;
            }

            if ((key == KeyEvent.VK_RIGHT) && (!leftDirection)) {
                rightDirection = true;
                upDirection = false;
                downDirection = false;
            }

            if ((key == KeyEvent.VK_UP) && (!downDirection)) {
                upDirection = true;
                rightDirection = false;
                leftDirection = false;
            }

            if ((key == KeyEvent.VK_DOWN) && (!upDirection)) {
                downDirection = true;
                rightDirection = false;
                leftDirection = false;
            }
        }
    }
}