import { unstable_noStore as noStore } from "next/cache";

export async function fetchUserById(id: string) {
    noStore();

    try {
        const response = await fetch(
            `http://localhost:8080/users/get-user-by-clerk-id?clerkId=${id}`
        );

        console.log(response);

        if (response.status === 404) {
            return null;
        }

        return await response.json();
    } catch (error) {
        console.error("Error fetching user", error);
        throw new Error("Failed to fetch user");
    }
}
