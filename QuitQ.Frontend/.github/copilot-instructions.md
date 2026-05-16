# Copilot instructions for QuitQ.Frontend

## Build, test, and lint commands
- Dev server: `npm run dev` (starts Vite dev server with HMR)
- Build for production: `npm run build` (Vite build)
- Preview production build locally: `npm run preview`
- Lint: `npm run lint` (runs `eslint .`)
- Lint a single file: `npx eslint "src\path\to\file.jsx"`
- Tests: No test runner or `test` script configured in package.json

(See package.json for exact script names.)

## High-level architecture
- Vite + React single-page app. Entry: `src/main.jsx`, root component: `src/App.jsx`.
- Routing: `src/routes/` centralizes route definitions; pages live in `src/pages/`.
- Reusable UI: `src/components/` (presentational + small widgets).
- Layouts: `src/layouts/` contains page shell components used by routes.
- State & auth: `src/context/` contains React Context providers; JWT-based auth utilities are present (jwt-decode) and Google OAuth client is included.
- Data & side-effects: `src/services/` and `src/api/` contain API wrappers and network code.
- Hooks & utils: `src/hooks/` for custom hooks (prefixed `use*`), `src/utils/` for helpers.
- Styling: Tailwind + Bootstrap present; tailwind config and styles live under `src/styles/` and `assets/`.

## Key conventions (repo-specific)
- Folder responsibilities: `pages/` for route-level components, `components/` for reusable pieces, `layouts/` for page scaffolding, `services/` for API functions, `context/` for providers.
- Custom hooks are placed under `src/hooks/` and named with a `use` prefix (e.g., `useAuth`).
- Services export functions for each resource (e.g., `getUsers`, `createUser`) and are consumed by pages/components rather than duplicating fetch logic.
- Routes are centralized under `src/routes/` (prefer importing route objects into the router rather than hardcoding paths in components).
- Linting: use `npm run lint` or `npx eslint <path>` for a single file; autofix with `--fix` when appropriate.

## Files consulted
- README.md (Vite + React template notes)
- package.json (scripts and dependencies)

## AI assistant / tooling files
- No existing Copilot/assistant configs (e.g., .github/copilot-instructions.md, CLAUDE.md, AGENTS.md) were found prior to this addition. If you have existing AI rules, add them alongside this file.

---

If any of the above summaries are incorrect or you want additional sections (e.g., CI commands, test setup recommendations, or Playwright/VRT setup steps), indicate which and the level of detail desired.
