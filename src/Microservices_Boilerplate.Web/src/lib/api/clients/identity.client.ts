import createClient from "openapi-fetch";
import type { paths } from "../contracts/identity.contract";


export const identityClinet = createClient<paths>({
  baseUrl: import.meta.env.VITE_IDENTITY_URL, 
});