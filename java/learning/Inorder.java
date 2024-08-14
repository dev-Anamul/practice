class Node {
    int data;
    Node left, right;

    Node(int val) {
        this.data = val;
        this.left = this.right = null;
    }
}

class Inorder {

    public static void printInOrder(Node node) {
        if (node == null)
            return;

        // go to the left
        printInOrder(node.left);

        // print the data
        System.out.println(node.data);

        // go to the right
        printInOrder(node.right);
    }

    public static void main(String args[]) {
        Node root = new Node(1);

        root.left = new Node(2);
        root.right = new Node(3);
        root.left.left = new Node(4);
        root.left.right = new Node(5);
        root.right.right = new Node(6);

        // print in order traversal of binary tree
        System.out.println("In Order Traversal of binary tree : ");
        printInOrder(root);
    }

}
