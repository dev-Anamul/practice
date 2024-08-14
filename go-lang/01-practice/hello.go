package main

import (
	"fmt"
	"strconv"
)

func main() {
	i := 42
	var j string
	j = strconv.Itoa(i)
	var isArray bool = true
	if isArray {
		fmt.Println("This is an array")
	}
	a := 10.0
	b := 3.0
	fmt.Println(a / b)
	fmt.Println(j)
	fmt.Println("Hello, World!")
}
