# Project Guidelines

This is a Godot demo repository used to test and refine an AI-assisted development workflow. Keep this file stable and high level; do not turn it into a changelog or implementation scratchpad.

## First Read

- Read `VISION.md` immediately before substantive work in this repo.
- Read `FILEMAP.md` at the start of every session to understand the current codebase layout.
- Treat `VISION.md` as the high-level direction, but verify actual behavior from the project files and Godot editor state.
- Keep `AGENTS.md` durable. Update it only when the workflow, stack, or broad conventions change.

## Tech Stack

- Engine: Godot 4.6.x.
- Language: C# for project-owned gameplay code unless a task gives a strong reason to use another language.
- Rendering: Forward Plus, as configured in `project.godot`.
- Physics: Jolt Physics for 3D physics, as configured in `project.godot`.
- AI tooling: Godot AI addon 2.6.0 with the local MCP endpoint configured in `.codex/config.toml`.

## Architecture

- This repo starts as a workflow template, not a complete game architecture.
- Keep game code separated from tooling. Project-owned scenes, scripts, resources, tests, and assets should live outside `addons/`.
- When adding real game code, prefer clear top-level folders such as:
  - `scenes/` for `.tscn` scene files.
  - `scripts/` for reusable C# source.
  - `resources/` for project-authored `.tres` and `.res` resources.
  - `assets/` for source art, audio, fonts, and imported media.
  - `tests/` for project tests.
- Keep scene ownership simple. A scene should have one clear responsibility and scripts should be named after the scene or system they drive.

## Godot AI MCP Workflow

- Before scene or project mutations, check the editor state with the Godot AI MCP server.
- If the project is running, stop it before editor-time scene edits unless the task is explicitly about runtime inspection.
- Prefer MCP scene, node, resource, script, material, input-map, animation, and test tools over ad hoc file edits when operating on Godot-owned data.
- Use direct file edits for documentation and hand-written scripts when that is clearer.
- After MCP scene edits, save the scene or project as appropriate and verify with editor state, scene hierarchy, logs, tests, or a short project run.

## File Map Maintenance

- After adding, deleting, or renaming source files, update `FILEMAP.md`.
- When refactoring moves code between files or changes a file's responsibility, update the relevant `FILEMAP.md` entry.
- Keep entries concise. One or two lines per file or directory is enough.
- For vendored or generated directories, document the directory purpose rather than every internal file.

## Code Style

- Follow the surrounding Godot and C# style in the files being edited.
- Use UTF-8 text. Prefer ASCII unless the file or feature already requires non-ASCII text.
- Use explicit node paths, exported properties, groups, and signals where they make scenes easier to inspect and modify through the editor.
- Keep scripts small and scene-oriented until there is enough real complexity to justify shared systems.
- Remove superseded concepts when replacing them. Do not leave legacy handlers, duplicate scenes, fallback scripts, or compatibility shims unless the user asks for a transition path.

## Build And Run

Open the project in Godot 4.6.x:

```bash
godot --path .
```

The project currently has no main scene. Create or open a scene before using run-mode workflows.

For AI-assisted editor work, start Godot with this project open and use the configured MCP endpoint:

```toml
[mcp_servers."godot-ai"]
url = "http://127.0.0.1:8000/mcp"
enabled = true
```

## Verification

- For documentation-only changes, review the edited files for accuracy against the current repo layout.
- For scene or asset changes, inspect the scene hierarchy and save the scene.
- For runtime behavior, run the project or current scene from Godot and inspect game/editor logs.
- For tests, use the project's C# test workflow once it exists. If a task specifically adds Godot AI GDScript tests, keep them isolated under `res://tests/test_*.gd`.
