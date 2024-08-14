
class Node {
    int data;
    Node left, right;

    Node(int val) {
        this.data = val;
        left = right = null;
    }

}

class Preorder {

    Node root;

    Preorder() {
        this.root = null;
    }

    public static void main(String args[]) {

        Preorder tree = new Preorder();

        tree.root = new Node(1);
        tree.root.left = new Node(2);
        tree.root.right = new Node(3);
        tree.root.left.left = new Node(4);
        tree.root.left.right = new Node(5);
        tree.root.right.right = new Node(6);

        System.out.println("Preorder traversal of binary tree is : ");
        tree.preorderTraversal(tree.root);

    }

    void preorderTraversal(Node node) {
        if (node == null) {
            return;
        }

        System.out.println(node.data);

        // left
        preorderTraversal(node.left);

        // right
        preorderTraversal(node.right);

    }

}
