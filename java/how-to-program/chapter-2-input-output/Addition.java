import java.util.Scanner;

public class Addition {
    public static void main(String[] args) {
        try (Scanner input = new Scanner(System.in)) {
            int num1;
            int num2;
            int sum;

            System.out.print("Enter first number: ");
            num1 = input.nextInt();

            System.out.print("Enter second number: ");
            num2 = input.nextInt();

            sum = num1 + num2;

            System.out.println("Sum is equals " + sum);
        }
    }

}
