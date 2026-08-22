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
* [RAG](#rag)
* [Chunking](#chunking)
* [How do Embedding, Vector Databses and Chunking, work together in a RAG system?](#how-do-embedding-vector-databses-and-chunking-work-together-in-a-rag-system)
* [LLM Tool Calling](#llm-tool-calling)


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

### RAG
RAG stands for Retrieval-Augmented Generation. RAG is a technique that allows an AI agent to use external knowledge without retraining or modifying its LLM.

Every AI agent typically uses an LLM that has already been trained and has knowledge learned during its training. 
However, the LLM doesn't automatically have access to our private documents or other external knowledge.

RAG helps us build a system that stores our documents and, whenever we need to ask something about them, retrieves the appropriate parts of the documents. 
These retrieved parts are then added to the context that we send to the LLM. The LLM can then generate an answer based on the retrieved information together with the knowledge it already has.

### Chunking
Why do we need to chunk documents in RAG systems? In a typical RAG system, we first divide the document into smaller chunks, 
then create an embedding for each chunk and store the vectors in a vector database.

The goal is not to make the chunks as small as possible. Instead, we want each chunk to be small enough to allow precise semantic search 
while still containing enough context to represent a meaningful piece of information.

Imagine you have a 20-page PDF document and only one page defines the company's days-off policy. 
If you save the entire PDF as a single piece, when the user asks about the days-off policy, 
the retrieval system may return the entire document. The LLM would then receive much more information than it needs.

However, if you chunk the PDF into 20 meaningful pieces, the retrieval system can find the chunk containing the days-off policy 
and send only that relevant information to the LLM. This can reduce the number of tokens sent to the LLM, reduce costs, and potentially make the answer faster and more focused.

### How do Embedding, Vector Databses and Chunking, work together in a RAG system?
First, we give some documents to the system. The system chunks the documents into smaller pieces.

Second, it sends each chunk to the embedding model, which generates a vector for each chunk.

Third, the system stores these vectors in a vector database such as Qdrant, usually together with their corresponding chunks and metadata.

Fourth, when a question arrives, the system sends the question to the embedding model and gets a vector representing the question. 
It then sends this vector to the vector database, which searches for vectors that are semantically similar to the question and returns the corresponding relevant chunks.

Finally, the system gives the original question together with the retrieved chunks to the LLM. The LLM uses the retrieved information, together with its pretrained knowledge, to generate the response.

### LLM Tool Calling
In a simple AI agent, the user sends a question to the agent, and the LLM generates an answer based on its pretrained knowledge. 
However, there are situations where the LLM cannot directly perform an operation or access the required information.

For example, if the user asks about tomorrow's weather, the LLM cannot know the actual forecast from its pretrained knowledge. It needs to call an external weather service.

For these situations, we can provide the agent with tools. A tool is a function or service that the agent can call to perform a specific task or access external information. 
For example, we can create tools for weather information, mathematical calculations, database queries, or calling external APIs.

When the agent receives a question, the LLM is given the available tools and their descriptions. 
The LLM determines whether it can answer the question using its own knowledge or whether it needs to call one of the available tools.

If a tool is needed, the LLM generates a tool call with the required parameters. The agent executes the tool and receives its result. 
The result is then provided back to the LLM, which uses it to generate the final answer.

If no tool is needed, the LLM can answer directly.