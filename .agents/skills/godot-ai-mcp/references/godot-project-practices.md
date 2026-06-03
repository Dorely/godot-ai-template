# Godot Project Practices

Use this reference when making design or implementation decisions in a Godot project. Prefer repo-local guidance when it is more specific.

## Scenes And Architecture

- Build small scenes with one clear responsibility. Think of a scene plus its root script as a reusable object with its own node structure, initialization, and internal signal connections.
- Keep reusable sub-scenes independent of their parent context when possible. If a child needs external behavior or data, have the parent wire it through exported properties, method calls, node references, or signal connections.
- Use signals for events that happened and that a parent or sibling may react to. Use direct method calls for commands where the caller intentionally starts behavior.
- Avoid hard-coded paths from reusable scenes into their future parents. If a node path must be used, expose it or resolve it from the owner/root that owns the relationship.
- Start simple with an entry scene that owns a world branch and a UI branch. Add subsystems only when the game actually needs them.
- Keep project-owned gameplay, tests, resources, and assets outside `addons/`; treat addons as tooling or vendored dependencies.

Official references:
- https://docs.godotengine.org/en/stable/tutorials/best_practices/scene_organization.html
- https://docs.godotengine.org/en/stable/tutorials/best_practices/what_are_godot_classes.html

## Resources, Autoloads, And Data

- Use nodes for behavior and engine participation. Use `Resource` objects for reusable data consumed by nodes.
- Prefer resources, owner/root state, scene-local nodes, or parent wiring before adding global state.
- Use autoloads for broad-scope systems that own their own state and are useful regardless of which scene is running, such as high-level save, dialogue, quest, or routing systems.
- Avoid autoloads that reach into many unrelated scenes or become generic managers for behavior that could remain scene-local.

Official references:
- https://docs.godotengine.org/en/stable/tutorials/scripting/resources.html
- https://docs.godotengine.org/en/stable/tutorials/best_practices/autoloads_versus_regular_nodes.html

## Files, Assets, And Version Control

- Godot uses the project filesystem directly. Use `res://` paths when referring to project resources.
- Organize files around maintainability for the project size. A clear top-level split such as `scenes/`, `scripts/`, `resources/`, `assets/`, and `tests/` is a good default for small and medium projects.
- Keep source assets in the project and let Godot import them. Commit the source asset and its generated `.import` metadata.
- Do not commit or hand-edit `.godot/`; it is editor cache and imported build output.
- Use `.gdignore` for folders inside the project tree that Godot should not import.
- Preserve LF line endings and UTF-8 text.

Official references:
- https://docs.godotengine.org/en/stable/tutorials/best_practices/project_organization.html
- https://docs.godotengine.org/en/stable/tutorials/assets_pipeline/import_process.html
- https://docs.godotengine.org/en/4.4/tutorials/best_practices/version_control_systems.html

## C# In Godot

- Use the .NET build of the Godot editor for C# projects.
- When bootstrapping or updating a C# project file, match the `Godot.NET.Sdk` package version to the connected Godot editor version.
- Match C# script class names to file names for attached scripts.
- Use `[Export]` properties for values designers or future editor automation should tune through the Inspector.
- Export typed node references or `NodePath` values instead of hard-coding parent-specific paths inside reusable child scenes.
- Remember that some string-based Godot APIs expect engine `snake_case` names. Prefer exposed `PropertyName`, `MethodName`, and `SignalName` constants when available.
- Reassign value-type properties after editing copies, such as `Position = Position with { X = 100.0f };`.
- Commit Godot-generated `.cs.uid` sidecars with their matching `.cs` files unless repo-local guidance says otherwise.
- Follow Godot's C# style where local convention is absent: UTF-8, LF endings, 4 spaces, Allman braces, PascalCase members/types, camelCase locals/parameters, and `_privateField` names.
- Be aware of platform limits for C# projects, especially lack of Godot 4 web export support.

Official references:
- https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_basics.html
- https://docs.godotengine.org/en/4.6/tutorials/scripting/c_sharp/c_sharp_exports.html
- https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_style_guide.html
