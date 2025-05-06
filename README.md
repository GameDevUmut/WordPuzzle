# Word Grid Game - Unity Project

A word-finding grid game inspired by Boggle, where players find words in a grid of letters. This repository showcases modern Unity development practices with clean architecture and robust systems.

## Technical Architecture

This project demonstrates several advanced Unity development concepts:

- **Additive Scene Structure** for optimized loading and scene management
- **Dependency Injection** framework (VContainer) for loose coupling and modular design
- **Unit Testing** with two test suites specifically for Grid generation and Trie data structure systems

## Core Technologies

### Libraries & Frameworks

- **Cysharp's UniTask** for low-overhead asynchronous operations in Unity
- **R3 Library** for implementing the Observer pattern through ReactiveProperties, providing dynamic UI updates
- **DOTween** for smooth animations and visual effects
- **Newtonsoft JSON** for data serialization and deserialization

### Performance Optimizations

- Strategic use of **Task.Run** for thread parallelization, particularly when building Trie structures and searching for words in the grid to maintain main thread performance
- **Addressable Assets** system for efficient resource loading and memory management

## Dictionary & Word Processing

- Integrated a comprehensive Turkish dictionary sourced from TDK (Turkish Language Association)
- Custom editor tool to convert dictionary data to an optimized JSON format
- Words limited to 3+ characters to improve gameplay balance and reduce excessive matches
- Dictionary is loaded as a text asset through the addressable system for efficient memory management

## User Interface

- **Responsive UI** design accommodating various aspect ratios and device types
- **Dynamic UI updates** powered by ReactiveProperties through the R3 library
- **Visual assets** created using generative AI (Imagen 3) and supplemented with UI asset packages

## Getting Started

1. Clone this repository
2. Open the project in Unity (version X.X.X or higher)
3. Install the required packages through the Package Manager
4. Open the main scene and press Play

## Future Development

The current architecture provides a solid foundation for extending the game with:
- Additional game modes
- Multiplayer functionality
- New visual themes
- Enhanced scoring systems
