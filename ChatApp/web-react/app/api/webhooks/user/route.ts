import { Webhook } from "svix";
import { cookies, headers } from "next/headers";
import {
    WebhookEvent,
    auth,
    clerkClient,
    currentUser,
} from "@clerk/nextjs/server";
import { revalidatePath } from "next/cache";
import { useClerk } from "@clerk/nextjs";
import { NextRequest } from "next/server";

export async function POST(req: NextRequest) {
    const WEBHOOK_SECRET = process.env.WEBHOOK_SECRET;

    if (!WEBHOOK_SECRET) {
        throw new Error(
            "Please add WEBHOOK_SECRET from Clerk Dashboard to .env or .env.local"
        );
    }

    const headerPayload = headers();
    const svix_id = headerPayload.get("svix-id");
    const svix_timestamp = headerPayload.get("svix-timestamp");
    const svix_signature = headerPayload.get("svix-signature");

    if (!svix_id || !svix_timestamp || !svix_signature) {
        return new Response("Error occurred -- no svix headers", {
            status: 400,
        });
    }

    const payload = await req.json();
    const body = JSON.stringify(payload);

    const wh = new Webhook(WEBHOOK_SECRET);

    let evt: WebhookEvent;

    try {
        evt = wh.verify(body, {
            "svix-id": svix_id,
            "svix-timestamp": svix_timestamp,
            "svix-signature": svix_signature,
        }) as WebhookEvent;
    } catch (err) {
        console.error("Error verifying webhook:", err);
        return new Response("Error occurred", {
            status: 400,
        });
    }

    const { id } = evt.data;
    const eventType = evt.type;

    console.log(`Webhook with and ID of ${id} and type of ${eventType}`);
    console.log("Webhook body:", body);

    if (eventType === "user.deleted") {
        const response = await fetch(
            `http://localhost:8080/users/delete-user?userId=${id}`,
            {
                method: "DELETE",
            }
        );

        console.log(response);
    } else if (eventType === "user.created") {
        const user = JSON.parse(body).data;

        // console.log(user);

        const data = {
            username: user.username,
            email: user.email_addresses[0].email_address,
            clerkId: id,
        };

        console.log(data);

        const response = await fetch(
            "http://localhost:8080/users/register-user",
            {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(data),
            }
        );

        console.log(response);
    }

    return new Response("", { status: 200 });
}
