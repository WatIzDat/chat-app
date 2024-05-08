import { clerkMiddleware, createRouteMatcher } from "@clerk/nextjs/server";
import { fetchJoinedDiscussionsByUser } from "@/app/lib/data";
import { NextResponse } from "next/server";

const isProtectedRoute = createRouteMatcher([
    "/my-discussions(.*)",
    "/discussion/(.*)",
    "/join-discussion/(.*)",
]);

const isDiscussionRoute = createRouteMatcher(["/discussion/(.*)"]);

const isJoinDiscussionRoute = createRouteMatcher(["/join-discussion/(.*)"]);

export default clerkMiddleware(async (auth, req) => {
    if (isProtectedRoute(req)) {
        auth().protect();
    }

    if (isDiscussionRoute(req)) {
        const urlSplit = req.url.split("/");
        const discussionId = urlSplit[urlSplit.length - 1];

        const joinedDiscussions: { id: string; name: string }[] =
            await fetchJoinedDiscussionsByUser(
                auth().sessionClaims?.userId as string
            );

        if (
            joinedDiscussions &&
            !joinedDiscussions.map((d) => d.id).includes(discussionId)
        ) {
            const url = req.nextUrl.clone();
            url.pathname = `join-discussion/${discussionId}`;

            return NextResponse.redirect(url);
        }
    }

    if (isJoinDiscussionRoute(req)) {
        const urlSplit = req.url.split("/");
        const discussionId = urlSplit[urlSplit.length - 1];

        const joinedDiscussions: { id: string; name: string }[] =
            await fetchJoinedDiscussionsByUser(
                auth().sessionClaims?.userId as string
            );

        if (
            joinedDiscussions &&
            joinedDiscussions.map((d) => d.id).includes(discussionId)
        ) {
            const url = req.nextUrl.clone();
            url.pathname = `discussion/${discussionId}`;

            return NextResponse.redirect(url);
        }
    }
});

export const config = {
    matcher: ["/((?!.*\\..*|_next).*)", "/", "/(api|trpc)(.*)"],
};
