# The Roleplay Master

## Introduction
This is my attempt at creating a feature-rich, immersive and realistic, thus complex, AI-Based Text Roleplaying Game.

## Notes
- Every component has a single responsibility and can be developed independently of the rest.
- The intermediary/router is built in Go and serves as an interface between all components.
  - The only reason there is a logging subcomponent within the intermediary is because it is logical for it to have its own separate logging functionality for debugging purposes, rather than relying on the logging component.
