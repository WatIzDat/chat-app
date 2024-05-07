export async function fetchUserById(id: string) {
    try {
        const response = await fetch(
            `http://localhost:8080/users/get-user-by-clerk-id?clerkId=${id}`
        );

        // console.log(response);

        if (response.status === 404) {
            return null;
        }

        return await response.json();
    } catch (error) {
        console.error("Error fetching user", error);
        throw new Error("Failed to fetch user");
    }
}

export async function fetchCreatedDiscussionsByUser(userId: string) {
    try {
        const response = await fetch(
            `http://localhost:8080/discussions/get-created-discussions-by-user?userId=${userId}`
        );

        return await response.json();
    } catch (error) {
        console.error("Error fetching discussions", error);
        throw new Error("Failed to fetch discussions");
    }
}

export async function fetchJoinedDiscussionsByUser(userId: string) {
    try {
        const response = await fetch(
            `http://localhost:8080/discussions/get-joined-discussions-by-user?userId=${userId}`
        );

        return await response.json();
    } catch (error) {
        console.error("Error fetching discussions", error);
        throw new Error("Failed to fetch discussions");
    }
}
