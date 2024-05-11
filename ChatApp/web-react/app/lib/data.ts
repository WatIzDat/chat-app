import { auth } from "@clerk/nextjs/server";

export async function fetchUserById(id: string) {
    try {
        const response = await fetch(
            `http://localhost:8080/users/get-user-by-clerk-id?clerkId=${id}`,
            {
                method: "GET",
                headers: {
                    Authorization: `Bearer ${await auth().getToken()}`,
                },
            }
        );

        if (!response.ok) {
            return null;
        }

        return await response.json();
    } catch (error) {
        console.error("Error fetching user", error);
        throw new Error("Failed to fetch user");
    }
}

export async function fetchDiscussionById(id: string) {
    try {
        const response = await fetch(
            `http://localhost:8080/discussions/get-discussion-by-id?id=${id}`,
            {
                method: "GET",
                headers: {
                    Authorization: `Bearer ${await auth().getToken()}`,
                },
            }
        );

        if (!response.ok) {
            return null;
        }

        return await response.json();
    } catch (error) {
        console.error("Error fetching discussion", error);
        throw new Error("Failed to fetch discussion");
    }
}

export async function fetchCreatedDiscussionsByUser() {
    try {
        const response = await fetch(
            `http://localhost:8080/discussions/get-created-discussions-by-user?userId=${
                auth().sessionClaims?.userId
            }`,
            {
                method: "GET",
                headers: {
                    Authorization: `Bearer ${await auth().getToken()}`,
                },
            }
        );

        if (!response.ok) {
            return null;
        }

        return await response.json();
    } catch (error) {
        console.error("Error fetching discussions", error);
        throw new Error("Failed to fetch discussions");
    }
}

export async function fetchJoinedDiscussionsByUser() {
    try {
        const response = await fetch(
            `http://localhost:8080/discussions/get-joined-discussions-by-user?userId=${
                auth().sessionClaims?.userId
            }`,
            {
                method: "GET",
                headers: {
                    Authorization: `Bearer ${await auth().getToken()}`,
                },
            }
        );

        if (!response.ok) {
            return null;
        }

        return await response.json();
    } catch (error) {
        console.error("Error fetching discussions", error);
        throw new Error("Failed to fetch discussions");
    }
}
