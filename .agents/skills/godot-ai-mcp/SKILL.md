---
name: godot-ai-mcp
description: Use for Godot Engine projects that have the Godot AI MCP server available. Trigger for scene, node, resource, material, script, input map, project setting, runtime inspection, editor log, screenshot, animation, audio, particle, UI, camera, signal, autoload, or test work in Godot; also use when troubleshooting Godot editor state or deciding whether to edit Godot-owned data through MCP versus files.
---

# Godot AI MCP

Use this skill to work safely in Godot projects through the Godot AI MCP server while preserving normal code-review and repo hygiene. Treat project-local instructions as authoritative: read `AGENTS.md`, `VISION.md`, and `FILEMAP.md` when they exist before substantive work.

## Quick Start

1. Inspect the repo first: read `project.godot`, relevant source files, and local guidance before changing anything.
2. Check the editor with `editor_state` before Godot scene, project, or resource mutations. If the project is playing, stop it before editor-time edits unless the task is specifically runtime inspection.
3. Prefer MCP tools for Godot-owned data: scenes, nodes, resources, materials, input maps, animations, particles, audio players, UI controls, cameras, signals, autoloads, project settings, screenshots, logs, and tests.
4. Prefer direct file edits for documentation, hand-written C# source, non-Godot text files, and changes where the MCP script tools do not support the file type.
5. Save through MCP after scene/project mutations and verify with the editor state, scene hierarchy, node properties, logs, screenshots, tests, or a short run.
6. Update local file maps or docs when adding, deleting, renaming, or moving source files.

## Tool Routing

- **Editor/session state**: use `session_manage`, `session_activate`, and `editor_state` before writes or when multiple Godot editors may be connected.
- **Scenes and nodes**: use `scene_manage`, `scene_open`, `scene_save`, `scene_get_hierarchy`, `node_create`, `node_manage`, `node_find`, and `node_get_properties`.
- **Properties**: call `node_get_properties` before `node_set_property`; use exact Godot property names and scene paths, not guessed names.
- **Grouped scene edits**: use `batch_execute` for related node/property/script operations that should succeed or fail together.
- **Resources and assets**: use `resource_manage`, `material_manage`, `theme_manage`, `audio_manage`, `particle_manage`, and `filesystem_manage` when the editor should import, save, assign, or inspect resources.
- **Gameplay scripts**: for C# projects, write `.cs` files directly with normal code-editing tools, let Godot import/build them, then attach or wire them with MCP if needed. `script_create` and `script_patch` are GDScript-only because they require `.gd`; `script_attach` can attach an already imported script resource.
- **Input, signals, and globals**: use `input_map_manage`, `signal_manage`, and `autoload_manage` instead of editing `project.godot` or scene connection data by hand.
- **Runtime inspection**: use `project_run`, `project_manage stop`, `game_manage`, and `logs_read` when validating behavior in the running game.
- **Visual verification**: use `editor_screenshot` for editor/game/camera views when scene layout or rendering matters.
- **Tests**: use `test_run` and `test_manage` when the project has Godot AI GDScript tests; use the repo's C# test workflow when one exists.

## Reference Files

Load only the reference needed for the task:

- `references/godot-project-practices.md`: use when designing scenes, project layout, C# code, resources, autoloads, assets, or version-control behavior.
- `references/godot-ai-mcp-workflows.md`: use when selecting MCP tools, handling editor readiness, performing scene/resource mutations, running the project, or verifying through logs/tests/screenshots.

## Operating Rules

- Keep project-owned game code outside `addons/` unless the task targets the addon itself.
- Keep scenes small and reusable. Prefer parent-wired dependencies, exported properties, and signals over hard-coded cross-scene paths.
- Use Resources for reusable data and only use autoloads for broad-scope systems that own their own state.
- Treat generated editor caches such as `.godot/` as disposable; do not hand-edit them.
- Commit source assets and Godot `.import` metadata when assets are added.
- If MCP is unavailable, say so clearly, use file inspection for planning, and avoid pretending editor-only checks were run.
