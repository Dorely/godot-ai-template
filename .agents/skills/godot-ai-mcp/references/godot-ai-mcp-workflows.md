# Godot AI MCP Workflows

Use this reference when operating through the Godot AI MCP server. Tool names reflect Godot AI addon 2.6.x.

## Starting State

- Read repo instructions first. In this demo repo, `AGENTS.md` requires reading `VISION.md` and `FILEMAP.md` before substantive work.
- Inspect `project.godot` and `.codex/config.toml` when behavior depends on engine version, renderer, physics, autoloads, plugins, or MCP endpoint.
- Call `editor_state` before editor-time scene, resource, or project mutations.
- If multiple editors are connected, call `session_manage` and `session_activate`.
- If `editor_state.is_playing` is true and the task is not runtime inspection, stop the project with `project_manage(op="stop")` before editor writes.
- If readiness is `no_scene`, create or open a scene before scene hierarchy edits. File-only work can continue.
- In this repo, the MCP endpoint is `http://127.0.0.1:8000/mcp`, the Godot AI addon is enabled, and the connected editor has reported Godot `4.6.3-stable`.

## Path And Property Rules

- Use scene paths relative to the edited scene root, such as `/Main/Player`, for scene and node tools.
- Use runtime paths only with runtime game inspection tools when the game is running.
- Use `res://` paths for project files and resources.
- Before setting a property, call `node_get_properties` or use a resource/class introspection tool. Godot property names often differ from intuition.
- For resources assigned to node properties, use `resource_manage`, `material_manage`, or `node_set_property` with valid `res://` resource paths or supported inline resource dictionaries.
- Use `batch_execute` for multi-step scene edits that should roll back together if a later step fails.

## Common Task Routes

- **Create or edit a scene**: `scene_manage create` or `scene_open`, then `scene_get_hierarchy`, node tools, resource/material tools, `scene_save`, and hierarchy/property verification.
- **Add nodes**: `node_create` for built-in node classes or PackedScene instancing; then set properties after inspecting valid names.
- **Move or rename nodes**: `node_manage` with rename, reparent, duplicate, move, or delete operations.
- **Create reusable resources**: `resource_manage create`, `material_manage create/apply_preset`, `theme_manage create`, or specialized resource helpers; assign with resource/material tools.
- **Configure input**: `input_map_manage` for actions and bindings instead of hand-editing `project.godot`.
- **Wire signals**: `signal_manage list/connect/disconnect` for scene connections.
- **Configure project settings**: `project_manage settings_get/settings_set`; use `autoload_manage` for autoload registration.
- **Add UI**: `ui_manage build_layout`, `set_anchor_preset`, `set_text`, and `draw_recipe`; use `theme_manage` for reusable theme resources.
- **Add camera/audio/animation/particles**: use `camera_manage`, `audio_manage`, `animation_create`/`animation_manage`, and `particle_manage` so subresources and editor-visible settings are created correctly.
- **Work with scripts**: use `script_create`, `script_patch`, and `script_manage` only for `.gd`. For C#, edit `.cs` files directly, let Godot import/build, then use `script_attach` if the script resource exists.

## C# Project Bootstrap

- Before relying on C# scripts at runtime, check whether the project has `.csproj` and `.sln` files. If not, bootstrap them before runtime validation.
- Match the `Godot.NET.Sdk` package version to the connected Godot editor version when creating or updating a C# project file.
- Ensure `application/config/features` includes `C#`; use `project_manage settings_get/settings_set` instead of hand-editing `project.godot` when possible.
- After adding or changing C# scripts, run the repo's C# build command, reimport the `.cs` files through `filesystem_manage`, and save affected scenes.
- `script_attach` can succeed before the runtime assembly is loadable. Verify by running the scene and checking `game_manage get_node_info` for a non-null `script` property on nodes that should use C# scripts.
- Track Godot-generated `.cs.uid` sidecars with their corresponding `.cs` files unless repo-local guidance says otherwise.

## Runtime And Verification

- For scene or resource edits, save with `scene_save` or the relevant project/resource tool, then verify with `scene_get_hierarchy`, `node_get_properties`, resource inspection, or a screenshot.
- For runtime behavior, run with `project_run` in `current`, `main`, or `custom` mode as appropriate. Stop first if switching scenes.
- Wait for `editor_state.game_capture_ready` before `game_manage` runtime inspection.
- Use `game_manage get_scene_tree`, `get_node_info`, `get_ui_elements`, and input simulation for runtime validation.
- MCP key simulation can be timing- and focus-sensitive, especially for code using `Input.is_action_just_pressed` / `Input.IsActionJustPressed`. Cross-check with `game_manage input_state`, runtime node inspection, or deterministic direct checks when needed.
- `editor_manage game_eval` can time out on `await` if the game window is not focused. If that happens, restart the run if needed and prefer direct state inspection or short deterministic eval snippets.
- Read logs with `logs_read(source="game")`, `logs_read(source="editor")`, or `logs_read(source="all")`. Use editor logs for parse/import/editor errors and game logs for runtime stdout/errors.
- Use `editor_screenshot` with `source="viewport"` for editor 3D views, `source="cinematic"` for Camera3D scene rendering, and `source="game"` for the running game framebuffer.
- Run `test_run` for Godot AI GDScript tests under `res://tests/test_*.gd`. Use the project's C# test workflow for C# tests when it exists.
- Stop the game with `project_manage(op="stop")` after runtime verification unless the user asked to leave it running.

## Verification Checklist

- **Scene edits**: save the scene, inspect hierarchy/properties, and use a screenshot or short run when rendering/layout matters.
- **C# edits**: build the project, reimport changed C# files, run the scene, and verify runtime nodes report the expected script.
- **Input edits**: list the input map and inspect runtime action state before treating gameplay behavior as broken.
- **Runtime checks**: inspect game/editor logs, confirm `editor_state` readiness, and stop the run when validation is complete.

## Offline Or Unavailable MCP

- If MCP tools are absent or the editor is disconnected, continue only with file inspection, planning, and direct text edits that do not require editor validation.
- Tell the user exactly which MCP/editor verification could not be run.
- Do not hand-edit generated scene/resource internals unless the user explicitly accepts the risk and there is no editor/MCP path available.
