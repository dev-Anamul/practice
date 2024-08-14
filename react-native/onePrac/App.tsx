import React from 'react';
import {Button, Text, View, StyleSheet} from 'react-native';

function App(): React.JSX.Element {
  const [count, setCount] = React.useState(0);

  const increment = () => {
    setCount(pre => pre + 1);
  };

  const decrement = () => {
    setCount(pre => pre - 1);
  };
  return (
    <View style={styles.container}>
      <Text>Counter</Text>
      <Text style={styles.text}>{count}</Text>
      <Button title="Increment" onPress={increment} />
      <Button title="Decrement" onPress={decrement} />
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  text: {
    fontSize: 50,
    textAlign: 'center',
    margin: 10,
  },
  button: {
    margin: 10,
  },
});

export default App;
