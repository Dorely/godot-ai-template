# DEMO_PLATFORMER - File Map

> Auto-maintained reference. Update this file when source files are added, removed, renamed, or significantly refactored.
> Read this file at the start of each session to understand the current project layout.

---

## Root

| File | Description |
|------|-------------|
| `.editorconfig` | Minimal editor configuration; currently declares UTF-8 text. |
| `.gitattributes` | Normalizes text files to LF line endings when the repo is under Git. |
| `.gitignore` | Ignores Godot-generated editor/cache output such as `.godot/`. |
| `AGENTS.md` | Stable workflow and coding guidance for AI agents and contributors. |
| `FILEMAP.md` | This file; concise map of the repository structure. |
| `VISION.md` | Intentionally small statement of the demo/workflow-validation goal. |
| `icon.svg` | Default Godot project icon source asset. |
| `icon.svg.import` | Godot import metadata for `icon.svg`. |
| `project.godot` | Godot project settings; enables the Godot AI addon and `_mcp_game_helper` autoload. |

## Local AI Configuration

| Path | Description |
|------|-------------|
| `.codex/config.toml` | Local Codex MCP configuration for the Godot AI server at `http://127.0.0.1:8000/mcp`. |
| `.agents/skills/godot-ai-mcp/` | Repo-scoped Codex skill for safe Godot editor work through the Godot AI MCP server. |
| `.agents/skills/godot-ai-mcp/SKILL.md` | Skill trigger metadata and core workflow for Godot AI MCP usage. |
| `.agents/skills/godot-ai-mcp/references/` | Focused reference docs for Godot project practices and MCP tool workflows. |
| `.agents/skills/godot-ai-mcp/agents/openai.yaml` | Skill UI metadata and Godot AI MCP dependency declaration. |

## Godot AI Addon

`addons/godot_ai/` is vendored tooling for MCP-driven Godot editor automation. Treat it as external/tooling code unless the task explicitly targets the addon.

| Path | Description |
|------|-------------|
| `addons/godot_ai/plugin.cfg` | Godot plugin manifest for the Godot AI addon. |
| `addons/godot_ai/plugin.gd` | Main editor plugin entry point. |
| `addons/godot_ai/README.md` | Addon documentation. |
| `addons/godot_ai/LICENSE` | Addon license. |
| `addons/godot_ai/dispatcher.gd` | Dispatches MCP tool requests to addon handlers. |
| `addons/godot_ai/tool_catalog.gd` | Catalog of MCP tools exposed by the addon. |
| `addons/godot_ai/connection.gd` | Connection layer for MCP communication. |
| `addons/godot_ai/client_configurator.gd` | Writes MCP client configuration for supported AI clients. |
| `addons/godot_ai/mcp_dock.gd` | Editor dock UI for MCP status and controls. |
| `addons/godot_ai/telemetry.gd` | Addon telemetry support. |
| `addons/godot_ai/update_reload_runner.gd` | Reload helper used by addon update flows. |
| `addons/godot_ai/clients/` | Client-specific configuration strategies for Codex, Claude, Cursor, Windsurf, Zed, and related tools. |
| `addons/godot_ai/debugger/` | Runtime debugger bridge used for game inspection and logging. |
| `addons/godot_ai/dock_panels/` | Editor dock panel scripts, including logs and port selection UI. |
| `addons/godot_ai/handlers/` | MCP operation handlers for scenes, nodes, resources, scripts, UI, audio, particles, materials, tests, and other Godot systems. |
| `addons/godot_ai/runtime/` | Runtime helper scripts, including the `_mcp_game_helper` autoload used for game logging and inspection. |
| `addons/godot_ai/testing/` | Lightweight GDScript test runner infrastructure. |
| `addons/godot_ai/utils/` | Shared utility scripts for paths, settings, logging, ports, server lifecycle, and error handling. |
| `addons/godot_ai/**/*.gd.uid` | Godot UID sidecar files for addon scripts. |

## Generated Or Ignored

| Path | Description |
|------|-------------|
| `.godot/` | Generated Godot editor cache, shader cache, imported data, and UID cache. Ignored; do not edit by hand. |
| `android/` | Generated/exported Android project output if created. Ignored by `.gitignore`. |

## Project-Owned Source

No project-owned gameplay scenes, scripts, resources, tests, or assets exist yet.

When they are added, document them here using concise entries grouped by directory.
