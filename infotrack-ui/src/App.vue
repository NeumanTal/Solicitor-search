<script setup lang="ts">
import { computed, ref } from "vue";
import { getReport } from "./Services/api";
import type { ReportResponse } from "./Models/ReportResponse";

const TOKENS = {
  spacing: "12px",
  radius: "8px",
  shadow: "0 2px 8px rgba(0,0,0,0.08)"
};

const locations = ref([
  "London",
  "London-bridge",
  "Birmingham",
  "Bristol",
  "Bradford",
  "Leeds",
  "Liverpool",
  "Liverpool-street",
  "Manchester",
  "Sheffield"
]);

const newLocation = ref("");

const result = ref<ReportResponse | null>(null);
const loading = ref(false);
const error = ref("");

type SortKey =
  | "name"
  | "location"
  | "address"
  | "rating"
  | "noOfReviews";

const sortKey = ref<SortKey>("rating");
const ascending = ref(false);


async function search() {
  loading.value = true;
  error.value = "";

  try {
    result.value = await getReport(locations.value);
  }
  catch (err) {
    console.error(err);
    error.value = "Unable to load solicitor data.";
  }
  finally {
    loading.value = false;
  }
}


function addLocation() {
  const value = newLocation.value.trim();

  if (!value) {
    return;
  }

  locations.value.push(value);
  newLocation.value = "";
}


function removeLocation(location: string) {
  locations.value =
    locations.value.filter(x => x !== location);
}


function sortBy(key: SortKey) {

  if (sortKey.value === key) {
    ascending.value = !ascending.value;
    return;
  }

  sortKey.value = key;
  ascending.value = true;
}


function arrow(key: SortKey) {

  if (sortKey.value !== key) {
    return "";
  }

  return ascending.value ? "↑" : "↓";
}

const sortedSolicitors = computed(() => {

  if (!result.value?.solicitors) {
    return [];
  }

  return [...result.value.solicitors].sort((a, b) => {

    const aValue = a[sortKey.value];
    const bValue = b[sortKey.value];


    if (aValue == null) {
      return 1;
    }

    if (bValue == null) {
      return -1;
    }


    let comparison = 0;


    if (
      sortKey.value === "rating" ||
      sortKey.value === "noOfReviews"
    ) {

      comparison =
        Number(aValue) - Number(bValue);

    } else {

      comparison =
        String(aValue)
          .toLowerCase()
          .localeCompare(
            String(bValue).toLowerCase()
          );

    }


    return ascending.value
      ? comparison
      : -comparison;

  });

});

</script>


<template>

<div class="page">

<h1>
  Solicitor Report
</h1>


<div class="panel">


<div class="locations">

<span
  v-for="location in locations"
  :key="location"
  class="tag"
>

{{ location }}

<button
  @click="removeLocation(location)"
>
×
</button>

</span>

</div>



<div class="add">

<input
  v-model="newLocation"
  placeholder="Add location"
  @keyup.enter="addLocation"
/>


<button @click="addLocation">
Add
</button>

</div>



<button
class="search"
@click="search"
>

{{ loading ? "Loading..." : "Search" }}

</button>


</div>



<div
v-if="error"
class="error"
>
{{ error }}
</div>



<div
v-if="!result && !loading"
class="empty"
>
Run a search to view solicitor results.
</div>



<div
v-if="result"
class="report"
>


<div class="cards">


<div class="card">
<h3>Total</h3>
<strong>{{ result.totalSolicitors }}</strong>
</div>


<div class="card">
<h3>Average</h3>

<strong>
{{ result.averageRating?.toFixed(2) ?? "-" }}
</strong>

</div>


<div class="card">
<h3>Top rated</h3>

<strong>
{{ result.topRatedSolicitor?.name ?? "-" }}
</strong>

</div>


</div>



<table>

<thead>

<tr>


<th @click="sortBy('name')">
<span class="sortable">
Name {{ arrow("name") }}
</span>
</th>


<th @click="sortBy('location')">
<span class="sortable">
Location {{ arrow("location") }}
</span>
</th>


<th @click="sortBy('address')">
<span class="sortable">
Address {{ arrow("address") }}
</span>
</th>


<th>
Phone
</th>


<th @click="sortBy('rating')">
<span class="sortable">
Rating {{ arrow("rating") }}
</span>
</th>


<th @click="sortBy('noOfReviews')">
<span class="sortable">
Reviews {{ arrow("noOfReviews") }}
</span>
</th>


<th>
Website
</th>


</tr>

</thead>


<tbody>


<tr
  v-for="(solicitor, index) in sortedSolicitors"
  :key="`${solicitor.name}-${solicitor.address}-${solicitor.phoneNumber}-${index}`"
>


<td>
{{ solicitor.name || 'Not Provided'}}
</td>


<td>
{{ solicitor.location || 'Not Provided'}}
</td>


<td>
{{ solicitor.address || 'Not Provided'}}
</td>


<td>
{{ solicitor.phoneNumber || 'Not Provided'}}
</td>


<td>
{{ solicitor.rating ?? "-" }}
</td>


<td>
{{ solicitor.noOfReviews ?? 0}}
</td>


<td>
  <a 
    v-if="solicitor.website" :href="solicitor.website" target="_blank">Website
  </a>
  <span v-else>Not Provided</span>
</td>


</tr>


</tbody>


</table>


</div>


</div>

</template>


<style scoped>

.page {
  padding: 30px;
  font-family: system-ui, Segoe UI, sans-serif;
}


.panel {

display:flex;
flex-direction:column;
gap:v-bind("TOKENS.spacing");

padding:20px;

border:1px solid #ddd;
border-radius:v-bind("TOKENS.radius");

box-shadow:v-bind("TOKENS.shadow");

}


.locations {
display:flex;
gap:8px;
flex-wrap:wrap;
}


.tag {

background:#eee;

padding:6px 12px;

border-radius:20px;

}


button {

padding:10px 16px;

border-radius:8px;

border:none;

cursor:pointer;

}


.add {

display:flex;

gap:10px;

}


input {

padding:10px;

border-radius:8px;

border:1px solid #ccc;

}


.cards {

display:flex;

gap:20px;

margin:25px 0;

}


.card {

padding:20px;

border:1px solid #ddd;

border-radius:8px;

min-width:150px;

}


table {

width:100%;

border-collapse:collapse;

}


th,
td {

padding:10px;

border-bottom:1px solid #ddd;

}


th {

cursor:pointer;

white-space:nowrap;

}


.sortable {

display:inline-flex;

align-items:center;

gap:4px;

}


tbody tr:hover {

background:#f7f7f7;

}


.error {

color:red;

margin:15px 0;

}


.empty {

margin-top:20px;

}

</style>