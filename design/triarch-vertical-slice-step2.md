# Triarch Vertical Slice Step 2 - AnyMMO Foundation

## Goal
Establish data-driven zone, risk, and death rules for Triarch using JSON + JSON Schema validation at startup.

## Data Sources
- `/data/*.json` are the authoritative definitions.
- `/data/schemas/*.schema.json` validate the JSON files.
- Unity loads the mirrored copies from `Assets/StreamingAssets/TriarchData`.
- Use `Tools > Triarch > Sync Data to StreamingAssets` in the editor to mirror data.

## Runtime Components
- `TriarchDataBootstrap` loads and validates data at startup.
- `TriarchZoneService` assigns zone/risk tier to players.
- `TriarchDeathService` applies risk-tier loot rules and spawns loot containers.
- `TriarchDebugUI` displays zone + risk, and exposes a debug kill button.

## Assumptions
- In-editor and server startup should fail fast on schema validation errors.
- Loot containers are minimal GameObjects with `TriarchLootContainer` and item stacks.
- Networking authority is enforced through the `TriarchDeathService.IsServerAuthority` flag until FishNet wiring is in place.
