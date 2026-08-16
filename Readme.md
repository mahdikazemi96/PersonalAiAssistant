# AI Agent With .Net 

![Static Badge](https://img.shields.io/badge/stack-.net_8-purple)
![Static Badge](https://img.shields.io/badge/vector_db-orange)
![Static Badge](https://img.shields.io/badge/Qdrant-red)
![Static Badge](https://img.shields.io/badge/Rag-blue)
![Static Badge](https://img.shields.io/badge/Embedding-green)
![Static Badge](https://img.shields.io/badge/Chunking-yellow)
![Static Badge](https://img.shields.io/badge/Citation-gray)
![Static Badge](https://img.shields.io/badge/agent_tool_calling-black)
![Static Badge](https://img.shields.io/badge/plugable_style-white)

## Table of Contents
* [About the repo](#about-the-repo)
* [Some important concepts](#some-important-concepts)
* [Embedding Model](#embedding-model)
* [Vector Database](#vector-database)


## About The Repo
As AI agents are an important part of software development, I have decided to run a simple project and develop an AI agent. 
Since I'm a .NET developer, I have decided to develop the project with .NET. So, this project is good for .NET developers who are not familiar with AI agents. 
Understanding this project can help them learn about it.

Also, I had an idea which can be a potential idea in the AI agent industry. Actually, I wanted to build an AI agent which is pluggable, 
where every developer can fetch the project, just write their tool, and add it to the project, without doing anything more. The agent runs and can call the tool.

## Some Important Concepts
I want to explain some important concepts in the AI agents world, then start coding. 
If you are a bit familiar with AI agents, you have probably heard about them. Understanding these concepts helps you design and develop AI agents more easily and better.

### Embedding Model
An embedding model converts text into a vector, which is a collection of numerical values. 
The values are not necessarily limited to a specific range and can also be negative.
Forexample this is a vector of a text: [0.12, -0.43, 0.87, ...]

An embedding model is trained to create numerical representations of the semantic characteristics of text. 
During training, the model learns relationships between words, phrases, and their surrounding context from a large amount of text.

When we give a piece of text to an embedding model, it generates a vector that represents the meaning and characteristics of that text. 
Texts with similar meanings tend to produce vectors that are closer to each other in the vector space.

### Vector Database
A Vector Database is a type of database designed to store and efficiently search vectors based on their similarity.

Typically, a vector is stored together with a payload containing information related to that vector, such as the original text, document name, or page number.

When we want to search for relevant information, we first convert our target text into a vector using an embedding model. 
We then send this vector to the Vector Database.

The Vector Database compares the query vector with the stored vectors and finds the vectors that are most similar to it. 
It then returns the matching vectors together with their associated payloads.

