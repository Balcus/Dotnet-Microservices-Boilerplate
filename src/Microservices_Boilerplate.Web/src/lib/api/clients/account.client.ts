import createClient from "openapi-fetch";
import type { paths } from "../contracts/account.contract";


export const accountClient = createClient<paths>({
  baseUrl: import.meta.env.VITE_ACCOUNT_URL, 
});