# The Roleplay Master

## Table of Contents
- [Introduction](#introduction)
- [Notes](#notes)
- [Remarks](#remarks)

## Introduction
This is my attempt at creating a feature-rich, immersive and realistic, thus complex, AI-Based Text Roleplaying Game.

## Notes
- Every component has a single responsibility and can be developed independently of the rest.
- The intermediary/router is built in Go and serves as an interface between all components.
  - The only reason there is a logging subcomponent within the intermediary is because it is logical for it to have its own separate logging functionality for debugging purposes, rather than relying on the logging component.

## Remarks
The reason I took a component-based architecture with intermediary is so that:
- I can learn how to make languages work together efficiently.
- I can learn a multitude of new languages without it affecting the system as a whole.
- Each component can be done with a language/framework that specializes in it.
- Each component can develop independently of the rest of the system, in doing so making it re-usable in other projects too.
  - Additionally, this promotes a better separation of concerns (Single Responsibility Principle on module/component level) and higher maintainability & flexibility.