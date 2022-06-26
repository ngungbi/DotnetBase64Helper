# Base64 Helper

Simplify and optimize Base64 conversion using Memory Span. Produces less memory allocation than normal method.

## Features

- Simplify Base64 operation
- Optimize memory allocation
- Auto-detect padding
- URI-safe conversion option*

## Installation

```
dotnet add package Ngb.Base64Helper
```

## Usage

Using package

```c#
using Ngb.Base64Helper;
```

1. Convert string to base64

```c#
/* this package */
string example = "example string"; 
string result = example.ToBase64String();

/* common method */
// byte[] bytes = Encoding.UTB8.GetBytes(example);
// string result = Convert.ToBase64String(bytes);
```

2. Convert Base64 string to byte array

```c#
string example = "lGsS0gYqxAy=";
byte[] result = example.ToByteArrayFromBase64();
```

3. Convert Base64 string to normal string

```c#
string example = "CbULL2heeng+7uAtgHDmcNSK8RiksA97/r6fjw5dPVI=";
string result = example.ToStringFromBase64();
```

4. Hash string to SHA256

```c#
string example = "Hello world";
string result = example.ToSha256Base64String();
```

# Benchmarks
Benchmarked using BenchmarkDotNet
```c#
[Benchmark]
public string TraditionalBase64() {
    var input = "Lorem ipsum dolor sit amet!";
    return Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
}

[Benchmark]
public string WithBase64Helper() {
    var input = "Lorem ipsum dolor sit amet!";
    return input.ToBase64String();
}
```
Results
```
|            Method |     Mean |    Error |   StdDev |  Gen 0 | Allocated |
|------------------ |---------:|---------:|---------:|-------:|----------:|
| TraditionalBase64 | 90.01 ns | 1.853 ns | 2.830 ns | 0.0362 |     152 B |
|  WithBase64Helper | 87.96 ns | 1.819 ns | 2.233 ns | 0.0229 |      96 B |
```
With this package it is slightly faster and allocated much less memory (exact performance result may vary)
